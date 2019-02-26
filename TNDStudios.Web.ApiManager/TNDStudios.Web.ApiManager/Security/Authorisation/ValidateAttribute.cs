using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TNDStudios.Web.ApiManager.Security.Authentication
{
    public class ValidateAttribute : TypeFilterAttribute
    {
        public ValidateAttribute(string Type, string Company, string Permission) : base(typeof(ValidateClaimFilter))
        {
            Arguments = new object[] { new Claim($"{Type}_{Company}".ToLower().Trim(), Permission.ToLower().Trim()) };
        }
    }
}
