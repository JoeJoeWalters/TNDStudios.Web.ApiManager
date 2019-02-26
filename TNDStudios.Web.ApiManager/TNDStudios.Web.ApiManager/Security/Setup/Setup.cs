using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNDStudios.Web.ApiManager.Security.Authentication;

namespace TNDStudios.Web.ApiManager.Security.Setup
{
    public static class Setup
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection serviceCollection, IUserAuthenticator userAuthenticator)
        {
            // Configure mixed authentication 
            serviceCollection.AddAuthentication("MixedAuthenticationHandler")
                .AddScheme<AuthenticationSchemeOptions, MixedAuthenticationHandler>("MixedAuthenticationHandler", null);

            // Configure DI for application services
            serviceCollection.AddScoped<IUserAuthenticator>(implementationFactory => { return userAuthenticator; });

            return serviceCollection;
        }
    }
}
