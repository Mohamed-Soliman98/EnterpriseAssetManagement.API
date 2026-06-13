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
                    FullName = "المدير العام للسيستم",
                    Department = "IT Management"
                };

                // بنحاول نكريت اليوزر
                var createPower = await userManager.CreateAsync(newAdmin, "Admin@2026!");

                if (createPower.Succeeded)
                {
                    // لو نجح بنربطه بالدور
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
                else
                {
              
                    var errors = string.Join(", ", createPower.Errors.Select(e => e.Description));
                    throw new Exception($"فشل إنشاء يوزر الـ Admin بسبب: {errors}");
                }
            }
        }
    }
}
