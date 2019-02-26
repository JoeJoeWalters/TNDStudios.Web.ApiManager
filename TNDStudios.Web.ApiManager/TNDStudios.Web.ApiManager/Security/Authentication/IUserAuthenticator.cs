using TNDStudios.Web.ApiManager.Security.Objects;

namespace TNDStudios.Web.ApiManager.Security.Authentication
{
    public interface IUserAuthenticator
    {
        SecurityUser AuthenticateToken(string securityToken);
    }
}