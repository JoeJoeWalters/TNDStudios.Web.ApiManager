using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using TNDStudios.Web.ApiManager.Documentation;
using TNDStudios.Web.ApiManager.Security.Authentication;

namespace TNDStudios.Web.ApiManager
{
    /// <summary>
    /// Static class to handle extending properites of .Net core setups
    /// </summary>
    public static class Setup
    {
        /// <summary>
        /// Extending the service collection so that the Startup class can initiate the
        /// authentication, authorisation and web api properties
        /// </summary>
        /// <param name="serviceCollection">The incoming service collection which may already have been modified</param>
        /// <param name="userAuthenticator">The implementation of the authenticator to authorise and authenticate users</param>
        /// <returns>The service collection returned modified with the authenticator injected in to it</returns>
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection serviceCollection, IUserAuthenticator userAuthenticator)
        {
            // Configure mixed authentication so the api validates requests against this handler
            serviceCollection.AddAuthentication("MixedAuthenticationHandler")
                .AddScheme<MixedAuthenticationOptions, MixedAuthenticationHandler>("MixedAuthenticationHandler", 
                    options => 
                    {
                        options.SaveTokens = true;
                    });

            // Configure DI for application services and inject the authenticator
            serviceCollection.AddScoped<IUserAuthenticator>(implementationFactory => { return userAuthenticator; });

            // Send back the modified service collection as it will be modified again by other handlers
            return serviceCollection;
        }

        public static IServiceCollection AddCustomVersioning(this IServiceCollection serviceCollection)
        {
            // Set up the Api versioning
            serviceCollection
                .AddVersionedApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });

            serviceCollection.AddApiVersioning(o =>
            {
                o.ApiVersionReader = new HeaderApiVersionReader("api-version");
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
            });


            // Register the Swagger generator, defining 1 or more Swagger documents
            serviceCollection.AddSwaggerGen(options =>
            {
                // resolve the IApiVersionDescriptionProvider service
                // note: that we have to build a temporary service provider here because one has not been created yet
                var provider = serviceCollection.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                // add a swagger document for each discovered API version
                // note: you might choose to skip or document deprecated API versions differently
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                }

                // add a custom operation filter which sets default values
                options.OperationFilter<SwaggerDefaultValues>();

                // integrate xml comments
                //options.IncludeXmlComments(XmlCommentsFilePath);
            });

            return serviceCollection;
        }

        public static IApplicationBuilder UseCustomVersioning(this IApplicationBuilder applicationBuilder, IApiVersionDescriptionProvider provider)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            applicationBuilder.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            applicationBuilder.UseSwaggerUI(options =>
            {
                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });

            return applicationBuilder;
        }

        static Info CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new Info()
            {
                Title = $"Salesforce Outbound API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Description = "Salesforce integration with Swagger, Swashbuckle, Input Formatting and API versioning.",
                Contact = new Contact() { Name = "Joe Walters", Email = "info@thenakeddeveloper.com" },
                TermsOfService = "Owned By TNDStudios",
                License = new License() { Name = "MIT", Url = "https://opensource.org/licenses/MIT" }
            };

            if (description.IsDeprecated)
            {
                info.Description += " <b color=\"red\">This API version has been deprecated</b>.";
            }

            return info;
        }
    }
}
