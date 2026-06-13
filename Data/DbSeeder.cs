using Microsoft.AspNetCore.Identity;

namespace EnterpriseAssetManagement.API.Data
{
    public class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync (IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = {"Admin", "IT_Manager"};

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            ///
            var adminUser = await userManager.FindByNameAsync("super.admin");
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = "super.admin",
                    Email = "admin@school.com",
                    FullName = "System Administrator",
                    Department = "IT Management"
                };

                // بنحاول نكريت اليوزر
                var createResult = await userManager.CreateAsync(newAdmin, "Admin@2026!");

                if (createResult.Succeeded)
                {
                    // لو نجح بنربطه بالدور
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
                else
                {
              
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create Admin user due to: {errors}");
                }
            }
        }
    }
}
