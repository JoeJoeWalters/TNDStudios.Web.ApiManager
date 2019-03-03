using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using TNDStudios.Web.ApiManager.Security.Objects;

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
        public ValidateAttribute(string Type, string Category, string Permission) : base(typeof(ValidateClaimFilter))
           => Arguments = new object[] { ToClaim(Type, Category, Permission) };

        /// <summary>
        /// Format the new claim properties in to a new claim object that can be pulled apart again
        /// later to analyse it
        /// </summary>
        public static Claim ToClaim(String Type, String Category, String Permission)
        {
            // Lookup For System
            String seperator = SecurityClaim.ClaimSeperator ?? "_";
            Claim result = new Claim($"{SecurityClaim.ClaimPrefixIdentifier}{seperator}{Type}{seperator}{Category}".ToLower().Trim(), Permission.ToLower().Trim());

            // Additional Properties so we don't have to decode the string to query it
            result.Properties[SecurityClaim.TypePropertyName] = Type;
            result.Properties[SecurityClaim.CategoryPropertyName] = Category;
            result.Properties[SecurityClaim.PermissionPropertyName] = Permission;

            return result;
        }
    }
}
