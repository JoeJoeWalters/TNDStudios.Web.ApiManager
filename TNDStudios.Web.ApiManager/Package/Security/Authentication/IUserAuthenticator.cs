using System;
using System.IO;
using System.Threading.Tasks;
using TNDStudios.Web.ApiManager.Security.Objects;

namespace TNDStudios.Web.ApiManager.Security.Authentication
{
    /// <summary>
    /// Interface to specify how user authenticator's should behave
    /// User authenticators handle the authentication of users when a request
    /// is made to the api
    /// </summary>
    public interface IUserAuthenticator
    {
        /// <summary>
        /// Take a token (usually from the auth token in the header) and validate the
        /// user
        /// </summary>
        /// <param name="securityToken">The security token, usually from the header</param>
        /// <returns>The user that was found and validated, a null will be returned if no user was validated</returns>
        Task<SecurityUser> AuthenticateToken(string securityToken);

        /// <summary>
        /// Refresh the list of cached users that are validated against
        /// </summary>
        /// <returns>If the refresh was successful</returns>
        Boolean RefreshAccessList();
        Boolean RefreshAccessList(Stream stream);
        Boolean RefreshAccessList(AccessControl accessControl);
    }
}