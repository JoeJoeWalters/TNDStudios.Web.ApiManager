using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TNDStudios.Web.ApiManager.Security.OAuth;
using TNDStudios.Web.ApiManager.Security.Objects;

namespace TNDStudios.Web.ApiManager.Security.Authentication
{
    /// <summary>
    /// Standard user authenticator that reads from a Json file in a given location
    /// </summary>
    public class UserAuthenticator : IUserAuthenticator
    {
        /// <summary>
        /// Local cached list of users
        /// </summary>
        private AccessControl accessControl = new AccessControl() { };

        public TokenValidationParameters JWTValidationParams { get; internal set; }

        public async Task<SecurityUser> AuthenticateOAuth(OAuthTokenRequest tokenRequest)
        {
            // Not a JWT encoded bearer so compare to the API Key instead
            try
            {
                switch (tokenRequest.Type)
                {
                    case OAuthTokenRequest.GrantType.client_credentials:

                        return accessControl
                            .Users
                            .Where(user =>
                            {
                                return
                                    user.ClientId == tokenRequest.ClientId &&
                                    user.ClientSecret == tokenRequest.ClientSecret &&
                                    user.Authentication.Contains(SecurityUser.AuthenticationType.oauth);
                            }).FirstOrDefault();

                    case OAuthTokenRequest.GrantType.password:

                        return accessControl
                            .Users
                            .Where(user =>
                            {
                                return
                                    user.Username == tokenRequest.Username &&
                                    user.Password == tokenRequest.Password &&
                                    user.Authentication.Contains(SecurityUser.AuthenticationType.oauth);
                            }).FirstOrDefault();

                    case OAuthTokenRequest.GrantType.authorization_code:

                        return accessControl
                            .Users
                            .Where(user =>
                            {
                                return
                                    user.Key == tokenRequest.Code &&
                                    user.Authentication.Contains(SecurityUser.AuthenticationType.oauth);
                            }).FirstOrDefault();

                    default:

                        return null;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Take a token (usually from the auth token in the header) and validate the
        /// user
        /// </summary>
        /// <param name="securityToken">The security token, usually from the header</param>
        /// <returns>The user that was found and validated, a null will be returned if no user was validated</returns>
        public async Task<SecurityUser> AuthenticateToken(String securityToken)
        {
            // Not authorised by default
            SecurityUser result = null;

            try
            {
                // Basic authentication requested
                AuthenticationHeaderValue authHeader = null;
                try
                {
                    authHeader = AuthenticationHeaderValue.Parse(securityToken);
                }
                catch
                {
                    throw new Exception("Authentication Header Is Malformed");
                }

                String token = authHeader.Parameter; // Get the token from the header

                switch (authHeader.Scheme.ToLower().Trim())
                {
                    case "oauth":
                    case "bearer":

                        // Expects bearer to be JWT encoded       
                        // https://jwt.io/
                        SecurityToken jwtToken = null;
                        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

                        try
                        {
                            jwtToken = tokenHandler.ReadToken(token);
                        }
                        catch { }

                        // No failure, must be a valid JWT encoded bearer
                        if (jwtToken != null)
                        {
                            // TODO: Get the user details from the JWT Token instead of the access control list
                            JwtSecurityToken jwtSecurityToken = (JwtSecurityToken)jwtToken;

                            // Validate the token and get the principal
                            SecurityToken validatedToken = null;
                            IPrincipal principal = null;
                            try
                            {
                                principal = tokenHandler.ValidateToken(
                                                            token,
                                                            JWTValidationParams,
                                                            out validatedToken);
                            }
                            catch { }

                            // Did we gain a principal?
                            if (principal != null && validatedToken != null)
                            {
                                // Transpose the principal and the token details to the user
                                return new SecurityUser()
                                {
                                    Id = jwtSecurityToken.Claims.Where(claim => claim.Type == "id").FirstOrDefault().Value,
                                    Key = String.Empty,
                                    Username = String.Empty,
                                    Authentication = new List<SecurityUser.AuthenticationType>()
                                    {
                                        SecurityUser.AuthenticationType.oauth
                                    },
                                    Claims = new List<SecurityClaim>()
                                    {
                                    }
                                };
                            }
                        }
                        else
                        {
                            // Not a JWT encoded bearer so compare to the API Key instead
                            result = accessControl
                                .Users
                                .Where(user =>
                                {
                                    return
                                        user.Key == token &&
                                        user.Authentication.Contains(SecurityUser.AuthenticationType.apikey);
                                }).FirstOrDefault();
                        }

                        break;

                    case "basic":

                        // Get the credentials from the basic authentication paramater part
                        var credentialBytes = Convert.FromBase64String(authHeader.Parameter);

                        // Split up the credentials in the parameter and check the length being the correct size
                        var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
                        if (credentials.Length == 2)
                        {
                            // Authorised?
                            result = accessControl
                                .Users
                                .Where(user =>
                                {
                                    return
                                        user.Password == credentials[1] &&
                                        user.Username == credentials[0] &&
                                        user.Authentication.Contains(SecurityUser.AuthenticationType.basic);
                                }).FirstOrDefault();
                        }

                        break;

                    case "apikey":

                        result = accessControl
                            .Users
                            .Where(user =>
                            {
                                return
                                    user.Key == token &&
                                    user.Authentication.Contains(SecurityUser.AuthenticationType.apikey);
                            }).FirstOrDefault();

                        break;

                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return await Task.FromResult<SecurityUser>(result);
        }


        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="tokenValidationParameters">Validation parameters for the JWT Tokens</param>
        public UserAuthenticator()
            => RefreshAccessList(); // Get the new access control list

        public UserAuthenticator(TokenValidationParameters tokenValidationParameters)
        {
            this.JWTValidationParams = tokenValidationParameters; // Assign the validator for the JWT tokens
            RefreshAccessList(); // Get the new access control list
        }

        /// <summary>
        /// Refresh the list of cached users that are validated against
        /// </summary>
        /// <returns>If the refresh was successful</returns>
        public Boolean RefreshAccessList()
            => RefreshAccessList(new AccessControl());

        public Boolean RefreshAccessList(Stream stream)
        {
            try
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return RefreshAccessList(
                        JsonConvert.DeserializeObject<AccessControl>(reader.ReadToEnd())
                        );
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Boolean RefreshAccessList(AccessControl accessControl)
        {
            this.accessControl = accessControl ?? new AccessControl() { };
            return true;
        }
    }
}
