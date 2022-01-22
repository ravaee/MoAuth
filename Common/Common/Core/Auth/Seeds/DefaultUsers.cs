using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common
{
    public static class DefaultUsers
    {
        private const string Default_Password = "Abcd@1234";

        private static ApplicationUser SuperAdminUser = new ApplicationUser()
        {
            FullName = "Mohamad Ravaei",
            UserName = "SuperAdminUser@Email.com",
            Email = "SuperAdminUser@Email.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        private static ApplicationUser AdminUser = new ApplicationUser()
        {
            FullName = "Saeed Gharib",
            UserName = "AdminUser@Email.com",
            Email = "AdminUser@Email.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        private static ApplicationUser BasicUser = new ApplicationUser()
        {
            FullName = "Omid Poorabbas",
            UserName = "BasicUser@Email.com",
            Email = "BasicUser@Email.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        public static async Task SeedSuperAdminUsersAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var user = await userManager.FindByEmailAsync(SuperAdminUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(SuperAdminUser, Default_Password);

                await userManager.AddToRoleAsync(SuperAdminUser, Roles.SuperAdmin.ToString());
                await userManager.AddToRoleAsync(AdminUser, Roles.Admin.ToString());
                await userManager.AddToRoleAsync(AdminUser, Roles.Basic.ToString());
            }

            await roleManager.SeedClaimsForSuperAdmin();
        }

        public static async Task SeedAdminUsersAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var user = await userManager.FindByEmailAsync(AdminUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(AdminUser, Default_Password);

                await userManager.AddToRoleAsync(AdminUser, Roles.Admin.ToString());
                await userManager.AddToRoleAsync(AdminUser, Roles.Basic.ToString());
            }
        }

        public static async Task SeedBasicUsersAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            var user = await userManager.FindByEmailAsync(BasicUser.Email);

            if (user == null)
            {
                await userManager.CreateAsync(BasicUser, Default_Password);

                await userManager.AddToRoleAsync(BasicUser, Roles.Basic.ToString());
            }
        }

        private async static Task SeedClaimsForSuperAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var superAdminRole = await roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());
            await roleManager.AddPermissionClaim(superAdminRole, "Product");
        }

        private async static Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissions(module);

            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("Permission", permission));
                }

            }

        }

    }
}
