using AutoMapper;
using MagFra_Gym.Gymbokning.Models.Entities;
using MagFra_Gym.Gymbokning.Models.ViewModels;
using System.Security.Claims;

namespace MagFra_Gym.Gymbokning.Models.MapperModels
{
    public class AttendingResolver : IValueResolver<GymClass, GymClassViewModel, bool>
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AttendingResolver(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public bool Resolve(GymClass source, GymClassViewModel destination, bool destMember, ResolutionContext context)
        {
            var result = source.UserGymClasses is null ? false :
                source.UserGymClasses.Any(u => u.applicationUserId
                == _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return result;
        }
    }
}
