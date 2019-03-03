using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TNDStudios.Web.ApiManager.Security;
using TNDStudios.Web.ApiManager.Security.Authentication;
using TNDStudios.Web.ApiManager.Security.Objects;

namespace Website
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Set up the authentication service with the appropriate authenticator implementation
            IUserAuthenticator userAuthenticator = new UserAuthenticator();
            userAuthenticator.RefreshAccessList(new AccessControl()
            {
                Users = new List<SecurityUser>()
                {
                    new SecurityUser()
                    {
                        Authentication = new List<string>()
                        {
                            "basic",
                            "apikey"
                        },
                        Claims = new List<SecurityClaim>()
                        {
                            new SecurityClaim()
                            {
                                Categories = new List<String>()
                                {
                                    "values"
                                },
                                Name = "admin",
                                Permissions = new List<String>()
                                {
                                    "read",
                                    "write"
                                }
                            }
                        },
                        Id = "7ac39504-53f1-47f5-96b9-3c2682962b8b",
                        Key = "a2ffaf61-fde6-4b5d-b69d-5697321ea668",
                        Password = "password",
                        Username = "username"
                    }
                }
            });
            services.AddCustomAuthentication(userAuthenticator);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
