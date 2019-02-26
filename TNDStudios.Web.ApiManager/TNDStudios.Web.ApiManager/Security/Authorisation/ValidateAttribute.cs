using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TNDStudios.Web.ApiManager.Security.Authentication
{
    /// <summary>
    /// Attribute that can be tagged to endoints to indicate what user
    /// claims are allowed to have access to the endpoint
    /// </summary>
    public class ValidateAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Constructor for the validation attribute so that the claims for a user can be validated
        /// </summary>
        /// <param name="Type">The resource that is being validated against (e.g. Admin section etc.)</param>
        /// <param name="Company">The company silo that the permission may be for</param>
        /// <param name="Permission">What permission is needed to gain access to this resource</param>
        public ValidateAttribute(string Type, string Company, string Permission) : base(typeof(ValidateClaimFilter))
        {
            // Format the incoming parameters in to a new claim that can be checked against the user's own claims
            Arguments = new object[] { new Claim($"{Type}_{Company}".ToLower().Trim(), Permission.ToLower().Trim()) };
        }
    }
}
