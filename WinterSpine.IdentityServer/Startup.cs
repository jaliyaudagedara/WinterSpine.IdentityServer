﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IdentityServer4;
using IdentityServer4.Test;
using System.Security.Claims;
using IdentityModel;
using WinterSpine.IdentityServer.Configs;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace WinterSpine.IdentityServer
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DbConnection");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ApplicationIdentityDbContext>(options => options.UseSqlServer(connectionString));

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationIdentityDbContext>();

            services.AddIdentityServer()
                .AddTemporarySigningCredential()
                .AddConfigurationStore(builder =>
                    builder.UseSqlServer(connectionString,
                        options => options.MigrationsAssembly(migrationsAssembly))) // Need MigrationsAssembly to run dotnet ef database update
                .AddOperationalStore(builder =>
                    builder.UseSqlServer(connectionString,
                        options => options.MigrationsAssembly(migrationsAssembly))) // Need MigrationsAssembly to run dotnet ef database update
                .AddAspNetIdentity<IdentityUser>();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            InitializeDatabase(app);

            app.UseIdentity();
            app.UseIdentityServer();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookies"
            });

            var options = new OpenIdConnectOptions
            {
                AuthenticationScheme = "oidc",
                SignInScheme = "Cookies",

                Authority = Configuration["Authentication:Authority"],
                RequireHttpsMetadata = false,

                ClientId = Configuration["Authentication:ClientId"],
                SaveTokens = true,

                TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                }
            };

            options.Scope.Add("email");
            options.Scope.Add("role");
            app.UseOpenIdConnectAuthentication(options);

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
                serviceScope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>().Database.Migrate();

                var configurationDbContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                foreach (var client in Config.GetClients())
                {
                    var _client = configurationDbContext.Clients.FirstOrDefault(c => c.ClientId == client.ClientId);
                    if (_client == null)
                    {
                        configurationDbContext.Clients.Add(client.ToEntity());
                    }
                }
                configurationDbContext.SaveChanges();

                foreach (var resource in Config.GetIdentityResources())
                {
                    var _resource = configurationDbContext.IdentityResources.FirstOrDefault(c => c.Name == resource.Name);
                    if (_resource == null)
                    {
                        configurationDbContext.IdentityResources.Add(resource.ToEntity());
                    }
                }
                configurationDbContext.SaveChanges();

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                foreach (TestUser user in Config.GetUsers())
                {
                    var identityUser = new IdentityUser(user.Username)
                    {
                        Id = user.SubjectId
                    };

                    foreach (var claim in user.Claims)
                    {
                        identityUser.Claims.Add(new IdentityUserClaim<string>
                        {
                            UserId = identityUser.Id,
                            ClaimType = claim.Type,
                            ClaimValue = claim.Value
                        });
                    }

                    var existingUser = userManager.FindByNameAsync(user.Username).Result;
                    if (existingUser == null)
                    {
                        userManager.CreateAsync(identityUser, user.Password).Wait();
                    }
                    else
                    {
                        userManager.DeleteAsync(existingUser).Wait();
                        userManager.CreateAsync(identityUser, user.Password).Wait();
                    }
                }
            }
        }
    }
}
