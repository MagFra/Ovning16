using System.ComponentModel.DataAnnotations;

namespace MagFra_Gym.Gymbokning.Models.ViewModels
{
    public class GymClassViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool Attending { get; set; }
    }
}
