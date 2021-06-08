using System.Collections.Generic;
using Geex.IdentityServer.Identity;

namespace Geex.IdentityServer.PredefinedConfigurations
{
    public static class IdentityPredefinedConfiguration
    {
        public const string AdministrationPolicy = "Administrator";

        public const string AdministrationRoleName = "Administrator";
        public static List<Role> Roles => new List<Role>()
        {
            new Role()
            {
                Name = AdministrationRoleName,
            }
        };

    }
}
