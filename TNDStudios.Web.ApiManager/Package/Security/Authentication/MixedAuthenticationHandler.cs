﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Authenticate the request against the cached user credentials
        /// </summary>
        /// <returns>The Success Or Failure Result Code</returns>
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
                StringValues header = new StringValues();
                try
                {
                    header = Request.Headers["Authorization"];
                }
                catch
                {
                    throw new Exception("No Authorisation Header Found");
                }

                // Do the authentication by passing it to the supplied user authenticator implementation
                user = userAuthenticator.AuthenticateToken(header);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }

            // No user was found / authenticated
            if (user == null)
                return AuthenticateResult.Fail("Could Authenticate The Given Credentials");

            // Generate a new identity and inject the claims in to the identity
            var identity = new ClaimsIdentity(user.Claims, Scheme.Name) { };

            // Set up the claims user and identifier for the system to use
            // Add the standard claim entries if they don't already exist
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
            identity.AddClaim(new Claim(ClaimTypes.Sid, user.Key));
            identity.AddClaim(new Claim(ClaimTypes.AuthenticationMethod,
                            String.Join(",",
                                user.Authentication.Select(auth => auth.ToString()) ??
                                new List<string>() { })));

            // Create the ticket required
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            // Return that the authentication was successful and return the authentication ticket
            return AuthenticateResult.Success(ticket);
        }
    }
}
