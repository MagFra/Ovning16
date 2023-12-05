using Microsoft.AspNetCore.Identity;

namespace MagFra_Gym.Gymbokning.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<ApplicationUserGymClass> applicationUserGymClass { get; set; } = new List<ApplicationUserGymClass>();
    }
}
