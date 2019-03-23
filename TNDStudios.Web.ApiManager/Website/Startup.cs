using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using TNDStudios.Web.ApiManager;
using TNDStudios.Web.ApiManager.Data.Soap;
using TNDStudios.Web.ApiManager.Security.Authentication;

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
            // Set up the authentication service with the appropriate authenticator implementation
            FileStream accessControl = File.OpenRead(Path.Combine(Environment.CurrentDirectory, "users.json"));
            IUserAuthenticator userAuthenticator = new UserAuthenticator();
            userAuthenticator.RefreshAccessList(accessControl);

            // Regular system setup
            services
                .AddCors()
                .AddLogging()
                .AddMvc(options =>
                    {
                        options.InputFormatters.Add(new SoapFormatter());
                    })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Custom service setup for the API Manager
            services
                .AddCustomAuthentication(userAuthenticator)
                .AddCustomVersioning();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            IApiVersionDescriptionProvider provider)
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

            // Custom app builder setup for the API Manager
            app.UseCustomVersioning(provider);
        }
    }
}
