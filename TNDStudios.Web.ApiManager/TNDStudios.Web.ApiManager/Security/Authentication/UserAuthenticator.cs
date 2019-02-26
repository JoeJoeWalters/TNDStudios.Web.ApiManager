using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TNDStudios.Web.ApiManager.Security.Objects;

namespace TNDStudios.Web.ApiManager.Security.Authentication
{
    public class UserAuthenticator : IUserAuthenticator
    {
        /// <summary>
        /// Local cached list of users
        /// </summary>
        private static AccessControl accessControl = new AccessControl()
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
                            Companies = new List<String>()
                            {
                                "ba",
                                "gui"
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
        };

        /// <summary>
        /// Authenticate a user by the security token provided (usually from the header value in the request)
        /// </summary>
        /// <param name="securityToken">The value of the security header</param>
        /// <returns>The authenticated user</returns>
        public SecurityUser AuthenticateToken(String securityToken)
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

                switch (authHeader.Scheme.ToLower().Trim())
                {
                    case "basic":

                        // Get the credentials from the basic authentication paramater part
                        var credentialBytes = Convert.FromBase64String(authHeader.Parameter);

                        // Split up the credentials in the parameter
                        var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');

                        // Retrieve the username and password to pass on to the authentication service
                        var username = credentials[0];
                        var password = credentials[1];

                        // Authorised?
                        result = accessControl
                            .Users
                            .Where(user =>
                            {
                                return
                                    user.Password == password &&
                                    user.Username == username;
                            }).FirstOrDefault();

                        break;

                    case "bearer": // Yes, Yes I know
                    case "apikey":

                        String token = authHeader.Parameter; // Get the token from the header

                        result = accessControl
                            .Users
                            .Where(user =>
                            {
                                return user.Key == token;
                            }).FirstOrDefault();

                        break;

                    case "oauth":

                        break;

                    default:

                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex; // Raise up the error so it can be logged
            }

            return result;
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="path">Path to the json file that holds the authentication / permission structure</param>
        public UserAuthenticator(String path)
        {

        }
    }
}
