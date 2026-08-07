using ChefNear.Domain.Entities;
using HomeChefMarketplace.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChefNear.Infrastructure.Seed
{
   
        public static class SeedData
        {
            public static async Task SeedAsync(IServiceProvider serviceProvider)
            {
                using var scope = serviceProvider.CreateScope();
                var services = scope.ServiceProvider;

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<User>>();

            string[] roles = Enum.GetNames(typeof(UserRole));

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var adminEmail = "admin@chefnear.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    adminUser = new Admin
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        DisplayName = "Admin",
                        Role = UserRole.Admin,
                        EmailConfirmed=true,
                        Status = UserStatus.Active
                    };

                    var result = await userManager.CreateAsync(adminUser, "Admin@123456");

                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            
        }
    }
}
