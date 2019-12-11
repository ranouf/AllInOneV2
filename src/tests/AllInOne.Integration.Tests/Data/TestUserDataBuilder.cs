﻿using AllInOne.Domains.Core.Identity;
using AllInOne.Domains.Core.Identity.Entities;
using AllInOne.Domains.Infrastructure.SqlServer;
using Xunit.Abstractions;

namespace AllInOne.Integration.Tests.Data
{
    public class TestUserDataBuilder : BaseDataBuilder
    {
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;

        public const string AdministratorEmail = "administrator@sidekickinteractive.com";
        public const string AdministratorFirstname = "John";
        public const string AdministratorLastname = "Smith";
        public const string ManagerEmail = "manager@sidekickinteractive.com";
        public const string UserEmail = "user@sidekickinteractive.com";
        public const string Password = "Password123#";

        public TestUserDataBuilder(
            AllInOneDbContext context,
            IUserManager userManager,
            IRoleManager roleManager,
            ITestOutputHelper output) : base(context, output)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public override void Seed()
        {
            var roles = new Role[]
            {
                new Role(Domains.Core.Constants.Roles.Administrator),
                new Role(Domains.Core.Constants.Roles.Manager),
                new Role(Domains.Core.Constants.Roles.User)
            };

            foreach (var role in roles)
            {
                _roleManager.CreateAsync(role).Wait();
            }
            Output.WriteLine($"{roles.Length} Roles have been created.");

            var users = new User[]
            {
                new User(AdministratorEmail, AdministratorFirstname, AdministratorLastname),
                new User(ManagerEmail, "Jack", "Wiliams"),
                new User(UserEmail, "Donald", "Duck")
            };

            for (int i = 0; i < users.Length; i++)
            {
                var user = users[i];
                _userManager.CreateAsync(user, Password, roles[i], sendEmail: false, raiseEvent: false).Wait();
            }
            Output.WriteLine($"{users.Length} Users have been created.");
        }
    }
}
