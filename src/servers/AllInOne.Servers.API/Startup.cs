using AllInOne.Common.Authentication.Extensions;
using AllInOne.Common.Settings.Extensions;
using AllInOne.Domains.Core.Identity.Configuration;
using AllInOne.Domains.Core.Identity.Entities;
using AllInOne.Domains.Infrastructure.SqlServer;
using AllInOne.Servers.API.Filters;
using AllInOne.Servers.API.Middlewares;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Reflection;

namespace AllInOne.Servers.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IContainer ApplicationContainer { get; private set; }

        public Startup(
            IConfiguration configuration
        )
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Settings
            services.AddOptions();
            services.ConfigureAndValidate<DefaultUserAccountsSettings>(Configuration);

            SetUpDataBase(services);

            // Dependancy Injection
            services.AddAutofac();

            // Identity
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AllInOneDbContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<User, Role, AllInOneDbContext, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityUserToken<Guid>, IdentityRoleClaim<Guid>>>()
                .AddRoleStore<RoleStore<Role, AllInOneDbContext, Guid, UserRole, IdentityRoleClaim<Guid>>>();

            //Caching response for middlewares
            services.AddResponseCaching();

            // Authentication
            services.RegisterAuthentication();

            // Insights
            services.AddApplicationInsightsTelemetry(Configuration);

            // Add framework services.
            services.AddControllers(options =>
            {
                //options.EnableEndpointRouting = false;
                options.Filters.AddService(typeof(ApiExceptionFilter));
                options.Filters.Add(typeof(ValidateModelStateAttribute));
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // Swagger-ui 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = $"{Common.Constants.Project.Name} API",
                    Version = "v1",
                    Description = $"Welcome to the marvellous {Common.Constants.Project.Name} API!",
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new[] { "readAccess", "writeAccess" }
                    }
                });
            });
            services.AddSwaggerDocument();

            // Profiling
            services.AddMemoryCache();
            services.AddMiniProfiler(options =>
                options.RouteBasePath = "/profiler"
            );

            // Automapper
            services.AddAutoMapper(typeof(Startup).Assembly);

            services.AddControllers();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<ApiModule>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            // Https
            if (!env.IsDevelopment())
            {
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            //Authentication
            app.UseAuthentication();
            app.UseAuthorization();

            //Caching
            app.UseResponseCaching();

            // Swagger-ui
            app.UseSwagger(c => c.RouteTemplate = "api-endpoints/{documentName}/swagger.json");
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api-endpoints";
                c.SwaggerEndpoint("v1/swagger.json", $"{Common.Constants.Project.Name} V1");
                c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream($"{Common.Constants.Project.Name}.Servers.API.Assets.SwaggerIndex.html");
            });

            // Profiling, url to see last profile check: http://localhost:XXXX/profiler/results
            app.UseMiniProfiler();
            app.UseMiddleware<RequestMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Redirect any non-API calls to the Angular application
            // so our application can handle the routing
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404
                    && !Path.HasExtension(context.Request.Path.Value)
                    && !context.Request.Path.Value.StartsWith("/api/"))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });
        }

        public virtual void SetUpDataBase(IServiceCollection services)
        {
            services.AddDbContext<AllInOneDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("Default"),
                    opt => opt.EnableRetryOnFailure()
                )
            );
        }
    }
}
