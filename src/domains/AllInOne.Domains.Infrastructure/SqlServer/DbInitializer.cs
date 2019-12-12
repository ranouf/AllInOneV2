﻿using AllInOne.Common.Logging;
using AllInOne.Domains.Core;
using AllInOne.Domains.Core.Identity;
using AllInOne.Domains.Core.Identity.Configuration;
using AllInOne.Domains.Core.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AllInOne.Domains.Infrastructure.SqlServer
{
    public static class DbInitializer
    {
        public async static Task InitializeAsync(IServiceProvider services, ILoggerService logger)
        {
            await services.MigrateDatabaseAsync(logger);
            await services.SeedRolesAsync(logger);
            await services.SeedUsersAsync(logger);
        }

        #region Private
        private static async Task MigrateDatabaseAsync(this IServiceProvider services, ILoggerService logger)
        {
            try
            {
                logger.LogInformation("Start database migration.");
                var context = services.GetRequiredService<AllInOneDbContext>();
                await context.Database.MigrateAsync();
                logger.LogInformation("Database migration has been done.");
            }
            catch (Exception ex)
            {
                logger.LogError("Database migration has failed.", ex);
                throw;
            }
        }

        private static async Task SeedRolesAsync(this IServiceProvider services, ILoggerService logger)
        {
            try
            {
                logger.LogInformation("Start seeding roles.");
                var context = services.GetRequiredService<AllInOneDbContext>();
                var roles = new string[] {
                    Constants.Roles.Administrator,
                    Constants.Roles.Manager,
                    Constants.Roles.User
                };
                var roleManager = services.GetRequiredService<IRoleManager>();
                foreach (var role in roles)
                {
                    if (!context.Roles.Any(r => r.Name == role))
                    {
                        await roleManager.CreateAsync(new Role(role));
                    }
                }
                await context.SaveChangesAsync();
                logger.LogInformation("Roles have been seeded.");
            }
            catch (Exception ex)
            {
                logger.LogError("Roles seeding has failed.", ex);
                throw;
            }
        }

        private static async Task SeedUsersAsync(this IServiceProvider services, ILoggerService logger)
        {
            try
            {
                logger.LogInformation("Start seeding users.");
                var context = services.GetRequiredService<AllInOneDbContext>();
                var defaultUserAccountsSettings = services.GetRequiredService<IOptions<DefaultUserAccountsSettings>>().Value;

                var roleManager = services.GetRequiredService<IRoleManager>();
                var userManager = services.GetRequiredService<IUserManager>();
                foreach (var userAccount in defaultUserAccountsSettings.UserAccounts)
                {
                    if (!context.Users.Any(u => u.Email == userAccount.Email))
                    {
                        var role = await roleManager.FindByNameAsync(userAccount.RoleName);
                        await userManager.CreateAsync(
                            new User(
                                userAccount.Email,
                                userAccount.Firstname,
                                userAccount.Lastname
                            ),
                            userAccount.Password,
                            role,
                            sendEmail: false,
                            raiseEvent: false
                        );
                    }
                }
                await context.SaveChangesAsync();
                logger.LogInformation("Users have been seeded.");
            }
            catch (Exception ex)
            {
                logger.LogError("Users seeding has failed.", ex);
                throw;
            }
        }
        #endregion
    }
}