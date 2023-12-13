using MagFra_Gym.Gymbokning.Models.ViewModels;

namespace MagFra_Gym.Gymbokning.Services
{
    public interface IFakeGymClassServices
    {
        Task<IEnumerable<GymClassViewModel>> GetGymClassesControllersAsynk(bool old = true, bool future = true);
    }
}