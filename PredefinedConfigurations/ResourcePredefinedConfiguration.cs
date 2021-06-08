using System.Collections.Generic;
using IdentityModel;
using IdentityServer4.Models;

namespace Geex.IdentityServer.PredefinedConfigurations
{
    public static class ResourcePredefinedConfiguration
    {
        public static IEnumerable<IdentityResource> IdentityResources=> new[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResources.Phone(),
            new IdentityResource()
            {
                Name = JwtClaimTypes.Role,
                DisplayName = "Your role",
                Emphasize = true,
                UserClaims = {JwtClaimTypes.Role},
            }
        };
    }
}