using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using TNDStudios.Web.ApiManager.Security.Authentication;

namespace TNDStudios.Web.ApiManager.Security.Setup
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
                .AddScheme<AuthenticationSchemeOptions, MixedAuthenticationHandler>("MixedAuthenticationHandler", null);

            // Configure DI for application services and inject the authenticator
            serviceCollection.AddScoped<IUserAuthenticator>(implementationFactory => { return userAuthenticator; });

            // Send back the modified service collection as it will be modified again by other handlers
            return serviceCollection;
        }
    }
}
