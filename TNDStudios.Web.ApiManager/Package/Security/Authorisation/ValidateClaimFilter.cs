using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;

namespace TNDStudios.Web.ApiManager.Security.Authentication
{
    /// <summary>
    /// Filter used by the authentication service to validate required claims for access
    /// against the user's context claims
    /// </summary>
    public class ValidateClaimFilter : IAuthorizationFilter
    {
        /// <summary>
        /// a reference to the incoming claim so that the context can be used
        /// across multiple methods
        /// </summary>
        readonly Claim _claim;

        /// <summary>
        /// Default Constructor to pass in the claim to be validated
        /// against the user claims
        /// </summary>
        /// <param name="claim">The claim from the Api Endpoint</param>
        public ValidateClaimFilter(Claim claim)
        {
            _claim = claim; // Store the claim to the local so that OnAuthorization can pick it up
        }

        /// <summary>
        /// Take the claim that is being validated against and validate it 
        /// against the context of the request (which contains the user context)
        /// </summary>
        /// <param name="context">The context of the request (including the user context)</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Claim has to be validated as it is not forbidden by default
            var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == _claim.Type && c.Value == _claim.Value);
            if (!hasClaim)
            {
                context.Result = new ForbidResult(); // No claim, update the context to make it negative
            }
        }
    }
}
