using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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
                        try
                        {
                            jwtToken = (new JwtSecurityTokenHandler()).ReadToken(token);
                        }
                        catch { }

                        // No failure, must be a valid JWT encoded bearer
                        if (jwtToken != null)
                        {
                            // TODO: Get the user details from the JWT Token instead of the access control list
                            JwtSecurityToken jwtSecurityToken = (JwtSecurityToken)jwtToken;
                        }
                        else
                        {
                            // Not a JWT encoded bearer so compare to the API Key instead
                            result = accessControl
                                .Users
                                .Where(user =>
                                {
                                    return user.Key == token;
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
                                        user.Username == credentials[0];
                                }).FirstOrDefault();
                        }

                        break;
                        
                    case "apikey":

                        result = accessControl
                            .Users
                            .Where(user =>
                            {
                                return user.Key == token;
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
        /// <param name="path">Path to the json file that holds the authentication / permission structure</param>
        public UserAuthenticator()
            => RefreshAccessList();

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
            catch
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
