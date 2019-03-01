using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using TNDStudios.Web.ApiManager.Security.Objects;

namespace TNDStudios.Web.ApiManager.Extensions
{
    public static class ClaimExtensions
    {

        /// <summary>
        /// Convert the claim object which is a standard object in to our custom object and decipher the pieces
        /// </summary>
        public static SecurityClaim ToSecurityClaim(this Claim claim)
            => new SecurityClaim()
            {
            };

        /// <summary>
        /// Generate and consolidate a list of claims to a list of security claims,
        /// the important part being how it consolidates the claims again
        /// </summary>
        public static List<SecurityClaim> ToSecurityClaims(this List<Claim> claims)
            => new List<SecurityClaim>()
            {
            };
    }
}
