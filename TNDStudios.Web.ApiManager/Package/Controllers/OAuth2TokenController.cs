using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using TNDStudios.Web.ApiManager.Security.Authentication;
using TNDStudios.Web.ApiManager.Security.OAuth;
using TNDStudios.Web.ApiManager.Security.Objects;

namespace TNDStudios.Web.ApiManager.Controllers
{
    // https://stackoverflow.com/questions/50992380/oauth2-client-authentication-with-multiple-users
    public class OAuth2TokenController : ControllerBase
    {
        // Logger Family (Not just the one)
        public ILogger Logger { get; internal set; }

        // Authentication Handler
        public IUserAuthenticator Authenticator { get; internal set; }

        // Token encoding details
        public String JWTKey { get; internal set; }
        public String JWTIssuer { get; internal set; }
        public String JWTAudience { get; internal set; }
        public Int16 JWTExpiry { get => 3600; }
        public SymmetricSecurityKey JWTSecurityKey { get; internal set; }
        public SigningCredentials JWTSigningCredentials { get; internal set; } 
        public TokenValidationParameters JWTValidationParams { get; internal set; }

        public OAuth2TokenController(ILogger logger, 
            IUserAuthenticator userAuthenticator, 
            String JWTKey,
            String JWTIssuer,
            String JWTAudience)
        {
            // Assign the logger family
            Logger = logger;

            // Assign the user authentication method to create JWT Tokens from
            Authenticator = userAuthenticator;

            // Set up the signing credentials for JWT Tokens
            this.JWTKey = JWTKey;
            this.JWTIssuer = JWTIssuer;
            this.JWTAudience = JWTAudience;
            JWTSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.JWTKey));
            JWTSigningCredentials = new SigningCredentials(JWTSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            JWTValidationParams = new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = this.JWTIssuer,
                ValidAudience = this.JWTAudience,
                IssuerSigningKey = JWTSecurityKey
            };
        }

        [Consumes(@"application/json")]
        [Produces(@"application/json")]
        [ResponseCache(NoStore = true)]
        [HttpPost]
        public virtual ActionResult Post(OAuthTokenRequest request)
        {
            // Check the client id and secret being asked for;
            SecurityUser securityUser = Authenticator.AuthenticateOAuth(request).Result;            
            if (securityUser != null)
            {
                // Generate a new JWT Header to wrap the token
                var header = new JwtHeader(JWTSigningCredentials);

                // Create the content of the JWT Token with the appropriate expiry date
                // and claims to identify who the user is and what they are able to do
                var payload = new JwtPayload(
                    this.JWTIssuer, 
                    this.JWTAudience, 
                    new List<Claim>()
                    {
                        new Claim("Roles", "Admin"),
                        new Claim("Roles", "Finance")
                    }, 
                    DateTime.Now, 
                    DateTime.Now.AddSeconds(this.JWTExpiry));

                // Generate the final tokem from the header and it's payload
                var secToken = new JwtSecurityToken(header, payload);
                
                // Token to String so you can use it in the client
                var handler = new JwtSecurityTokenHandler();
                var tokenString = handler.WriteToken(secToken);

                /*
                var token = handler.ReadJwtToken(tokenString);
                String decoded = token.Payload.SerializeToJson();
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken validatedToken;
                IPrincipal principal = tokenHandler.ValidateToken(tokenString, validationParameters, out validatedToken);
                */

                return new OkObjectResult(
                    new OAuthTokenSuccess()
                    {
                        AccessToken = tokenString,
                        ExpiresIn = this.JWTExpiry,
                        RefreshToken = tokenString,
                        Scope = "test",
                        TokenType = "bearer"
                    });
            }
            else
            {
                return new BadRequestObjectResult(
                    new OAuthTokenFailure()
                    {
                        Reason = OAuthTokenFailure.ReasonType.unauthorized_client,
                        ReasonDescription = "Reason for the failure here",
                        ReasonUri = "url of the failure code"
                    });
            }
        }
    }
}
