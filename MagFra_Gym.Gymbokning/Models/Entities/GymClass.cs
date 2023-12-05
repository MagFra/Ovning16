using System.ComponentModel.DataAnnotations;

namespace MagFra_Gym.Gymbokning.Models.Entities
{
    public class GymClass
    {
        public Guid Id { get; set; }
        [MaxLength(64)]
        public string Name { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EndTime { get { return StartTime + Duration; } }
        [MaxLength(1024)]
        public string Description { get; set; } = string.Empty;



        ICollection<ApplicationUserGymClass> UserGymClasses { get; set; } = new List<ApplicationUserGymClass>();
    }
}
