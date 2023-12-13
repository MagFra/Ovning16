using AutoMapper;
using MagFra_Gym.Gymbokning.Models.Entities;
using MagFra_Gym.Gymbokning.Models.ViewModels;

namespace MagFra_Gym.Gymbokning.Models.MapperModels
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<GymClass, GymClassViewModel>()
            .ForMember(dest => dest.Attending, from => from.MapFrom<AttendingResolver>());

            CreateMap<IEnumerable<GymClass>, WraperGymClassesModel>()
            .ForMember(dest => dest.ListOfGymClasses, from => from.MapFrom(g => g.ToList()));
        }
    }
}
