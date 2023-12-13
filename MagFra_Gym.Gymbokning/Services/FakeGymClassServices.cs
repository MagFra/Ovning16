using AutoMapper;
using MagFra_Gym.Gymbokning.Controllers;
using MagFra_Gym.Gymbokning.Data;
using MagFra_Gym.Gymbokning.Models.Entities;
using MagFra_Gym.Gymbokning.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MagFra_Gym.Gymbokning.Services
{
    public class FakeGymClassServices : IFakeGymClassServices
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public FakeGymClassServices(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GymClassViewModel>> GetGymClassesControllersAsynk(bool old = true, bool future = true)
        {
            var gymClasses = await _context.GymClass.ToListAsync();
            var wrappedGymClasses = _mapper.Map<WraperGymClassesModel>(gymClasses);
            var model = wrappedGymClasses.ListOfGymClasses.ToList();
            return model;
        }
    }
}
