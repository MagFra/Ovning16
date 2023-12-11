using Microsoft.AspNetCore.Identity;
using MagFra_Gym.Gymbokning.Models.Entities;
using MagFra_Gym.Gymbokning.Data.Migrations;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
namespace MagFra_Gym.Gymbokning.Data
{
    public class SeedData
    {
        public static ApplicationDbContext db = default!;
        public static UserManager<ApplicationUser> userManager = default!;
        public static RoleManager<IdentityRole> roleManager = default!;

        private static IEnumerable<Guid> classesId = null!;

        public static async Task InitAsync(ApplicationDbContext context, IServiceProvider services)
        {
            db = context;

            if (db.Roles.Any()) return;

            userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            var roleNames = new[] { "Admin", "User" };
            await AddRolesAsync(roleNames);


            DateTime date1 = DateTime.ParseExact("2023-12-13 06:45:00", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            DateTime date2 = DateTime.ParseExact("2023-12-24 15:00:00", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            DateTime date3 = DateTime.ParseExact("2023-11-30 13:10:00", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            DateTime date4 = DateTime.ParseExact("2024-02-12 09:30:00", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

            var passes = new (string, DateTime, TimeSpan, string)[] {
                ("Luciastretch", date1 , TimeSpan.FromHours(0.25), "En class för att strecha inför Luciatåget."),
                ("Svensk jul", date2 , TimeSpan.FromHours(0.25), "Kalle anka och hans vänner önskar en \"Good Jul!\""),
                ("En gammal klass", date3 , TimeSpan.FromHours(0.25), "En class för för länge sedan."),
                ("En klass nästa år", date4 , TimeSpan.FromHours(0.25), "En class i februari nästa år."),
            };

            classesId = await AddClassesAsync(passes);

            var users = new (string, string, string, string, string?)[] {
                ("admin@gym.se", "%T0lss1t5", "Admin", "Adminson", "Admin"),
                ("user@gym.se","%T0lss1t5", "User", "Userson", "User"),
                ("member1@gym.se","%T0lss1t5", "Member1", "Memberson", null),
                ("member2@gym.se","%T0lss1t5", "Member2", "Membersson", null)
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

        private static async Task<IEnumerable<Guid>> AddClassesAsync((string, DateTime, TimeSpan, string)[] passes)
        {
            string name, description; DateTime startTime; TimeSpan duration;
            List<Guid> classes = new List<Guid>(0);

            foreach (var klass in passes)
            {
                (name, startTime, duration, description) = klass;
                var newClass = new GymClass
                {
                    Name = name,
                    StartTime = startTime,
                    Duration = duration,
                    Description = description,
                };
                var result = await db.AddAsync<GymClass>(newClass);
                await db.SaveChangesAsync();
                var temp = await db.GymClass.Where(g => g.Name.Equals(name)).FirstOrDefaultAsync();

                classes.Add(temp!.Id);
            }

            return classes;
        }

        //#####################################################################################

        private static async Task AddUsersAsync((string, string, string, string, string?)[] users)
        {
            string email, pw, firstName, lastName; string? role;

            foreach (var user in users)
            {
                (email, pw, firstName, lastName, role) = user!;
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

                if (role != null)
                {
                    await AddUserToRoleAsync(newUser, role);
                }
                var tempUser = await userManager.FindByEmailAsync(email);
                if (tempUser != null)
                {
                    var id = tempUser.Id;
                    foreach(var gymClassId in classesId)
                    {
                        var appUserGymClass = new ApplicationUserGymClass { applicationUserId = id, gymClassId = gymClassId };
                        db.ApplicationUserGymClass.Add(appUserGymClass);
                        db.SaveChanges();
                    }
                }
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
