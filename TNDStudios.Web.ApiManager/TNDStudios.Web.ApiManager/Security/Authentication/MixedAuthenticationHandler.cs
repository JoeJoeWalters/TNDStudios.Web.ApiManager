using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TNDStudios.Web.ApiManager.Security.Objects;

namespace TNDStudios.Web.ApiManager.Security.Authentication
{
    public class MixedAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary>
        /// Local reference to the user authenticator
        /// </summary>
        private IUserAuthenticator userAuthenticator;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MixedAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserAuthenticator userAuthenticator
            ) : base(options, loggerFactory, encoder, clock)
        {
            // Assign the user authenticator to use (could be database, json file etc.)
            this.userAuthenticator = userAuthenticator;
        }

        /// <summary>
        /// Authenticate the request
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // The security user that is found from the authentication process
            SecurityUser user = null;

            // Is there an authorisation header to cehck against?
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            // Try and parse the authorization header
            try
            {
                // Get the authorisation header
                StringValues header = new StringValues();
                try
                {
                    // Try and get the header from the request object
                    header = Request.Headers["Authorization"];
                }
                catch
                {
                    throw new Exception("No Authorisation Header Found"); // No header was found
                }

                // Do the authentication by passing it to the supplied user authenticator implementation
                user = userAuthenticator.AuthenticateToken(header);
            }
            catch (Exception ex)
            {
                // Something went wrong with the type or format of the header used to authorise
                return AuthenticateResult.Fail(ex.Message);
            }

            // No user was found / authenticated
            if (user == null)
                return AuthenticateResult.Fail("Could Authenticate The Given Credentials");

            // Set up the claims user and identifier for the system to use
            List<Claim> claims = new List<Claim> {

                // Add the standard claim entries
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Id),
            };

            // Loop each user claim
            user.Claims.ForEach(claim =>
            {
                // .. for each company they have access to ..
                claim.Companies.ForEach(company =>
                {
                    // .. for each permission granted ..
                    claim.Permissions.ForEach(permission =>
                    {
                        // .. add the new claim
                        claims.Add(new Claim($"{claim.Name}_{company}", permission));
                    });
                });
            });

            // Generate a new identity
            var identity = new ClaimsIdentity(claims, Scheme.Name);

            // Create the principal based on the identity
            var principal = new ClaimsPrincipal(identity);

            // Create the required authentication ticket based on the underlaying scheme
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            // Return that the authentication was successful and return the authentication ticket
            return AuthenticateResult.Success(ticket);
        }
    }
}
