using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MagFra_Gym.Gymbokning.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = default!;


        [Required]
        [MaxLength(50)]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = default!;


        [Display(Name = "Full name")]
        public string FullName => FirstName + " " + LastName;
        public DateTime TimeOfRegistration { get; set; }
        public ICollection<ApplicationUserGymClass> applicationUserGymClass { get; set; } = new List<ApplicationUserGymClass>();
    }
}
