using MagFra_Gym.Gymbokning.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MagFra_Gym.Gymbokning.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public DbSet<ApplicationUser> ApplicationUser => Set<ApplicationUser>();
        public DbSet<ApplicationUserGymClass> ApplicationUserGymClass => Set<ApplicationUserGymClass>();
        public DbSet<GymClass> GymClass => Set<GymClass>();

        //######################################################################################

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //######################################################################################

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserGymClass>().HasKey(a => new { a.gymClassId, a.applicationUserId });
        }
    }
}
