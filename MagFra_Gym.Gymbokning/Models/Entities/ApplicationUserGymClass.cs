using System.ComponentModel.DataAnnotations;

namespace MagFra_Gym.Gymbokning.Models.Entities
{
    public class ApplicationUserGymClass
    {
        public Guid gymClassId { get; set; }
        public GymClass gymClass { get; set; } = default!;

        [MaxLength(450)]
        public string applicationUserId { get; set; } = default!;
        public ApplicationUser applicationUser { get; set; } = default!;
    }
}
