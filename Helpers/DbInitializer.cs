using Final_Back.Constants;
using Final_Back.Models;
using Microsoft.AspNetCore.Identity;

namespace Final_Back.Helpers
{
    public  static class DbInitializer
    {

        public async static Task SeedAsync(UserManager<User> userManager,RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString(),
                    });
                }
            }
            User admin = new()
            {
                Fullname = "Dilara Huseynova",
                UserName = "admin",
                Email = "huseynova.8885@gmail.com",
            };
            if (await userManager.FindByNameAsync(admin.UserName) == null) 
            {
                var result = await userManager.CreateAsync(admin, "Admin123");
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        throw new Exception(error.Description);
                    }
                }
                await userManager.AddToRoleAsync(admin, UserRoles.SuperAdmin.ToString());

            }

        }

    }
}
