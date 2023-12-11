using Microsoft.AspNetCore.Identity;
using MagFra_Gym.Gymbokning.Models.Entities;
using MagFra_Gym.Gymbokning.Data.Migrations;
namespace MagFra_Gym.Gymbokning.Data
{
    public class SeedData
    {
        public static ApplicationDbContext db = default!;
        public static UserManager<ApplicationUser> userManager = default!;
        public static RoleManager<IdentityRole> roleManager = default!;

        public static async Task InitAsync(ApplicationDbContext context, IServiceProvider services)
        {
            db = context;

            if (db.Roles.Any()) return;

            userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            var roleNames = new[] { "Admin", "User" };
            await AddRolesAsync(roleNames);

            var users = new[] {
                                   ("admin@gym.se", "%T0lss1t5", "Admin", "Adminson", "Admin"),
                                   ("user@gym.se","%T0lss1t5", "User", "Userson", "User")
                              };

            await AddUsersAsync(users);

        }

        //#####################################################################################

        private static async Task AddRolesAsync(string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        //#####################################################################################

        private static async Task AddUsersAsync((string, string, string, string, string)[] users)
        {
            foreach (var user in users)
            {
                string email, pw, firstName, lastName, role;
                (email, pw, firstName, lastName, role) = user;
                if (await userManager.FindByEmailAsync(email) != null) continue;
                var newUser = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = firstName,
                    LastName = lastName,
                    TimeOfRegistration = DateTime.Now,
                };
                var result = await userManager.CreateAsync(newUser, pw);

                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

                await AddUserToRoleAsync(newUser, role);
            }

        }

        //#####################################################################################

        private static async Task AddUserToRoleAsync(ApplicationUser user, string roleName)
        {
            if (!await userManager.IsInRoleAsync(user, roleName))
            {
                var result = await userManager.AddToRoleAsync(user, roleName);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }
    }
}
