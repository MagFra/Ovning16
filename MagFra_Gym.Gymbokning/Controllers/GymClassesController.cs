﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MagFra_Gym.Gymbokning.Data;
using MagFra_Gym.Gymbokning.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using MagFra_Gym.Gymbokning.Models.ViewModels;
using MagFra_Gym.Gymbokning.Services;

namespace MagFra_Gym.Gymbokning.Controllers
{
    public class GymClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IFakeGymClassServices _fakeGymClassServices;

        public GymClassesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper, IFakeGymClassServices fakeGymClassServices)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _fakeGymClassServices = fakeGymClassServices;
        }

        //#####################################################################################

        public IActionResult Privacy()
        {
            return View();
        }

        //#####################################################################################

        // GET: GymClasses
        public async Task<IActionResult> Index()
        {
            return View(await _fakeGymClassServices.GetGymClassesControllersAsynk());
        }

        //#####################################################################################

        // GET: GymClasses/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));

            var gymClassWithAtendees = await _context.GymClass
                .Where(m => m.Id == id)
                .Include(a => a.UserGymClasses)
                .ThenInclude(c => c.applicationUser)
                .FirstOrDefaultAsync();

            if (gymClassWithAtendees == null) return RedirectToAction(nameof(Index));

            return View(gymClassWithAtendees);
        }

        //#####################################################################################

        // GET: GymClasses/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                gymClass.Id = Guid.NewGuid();
                _context.Add(gymClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        //#####################################################################################

        // GET: GymClasses/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClass.FindAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (id != gymClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gymClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymClassExists(gymClass.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        //#####################################################################################

        // GET: GymClasses/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await _context.GymClass
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // POST: GymClasses/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var gymClass = await _context.GymClass.FindAsync(id);
            if (gymClass != null)
            {
                _context.GymClass.Remove(gymClass);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //#####################################################################################

        private bool GymClassExists(Guid id)
        {
            return _context.GymClass.Any(e => e.Id == id);
        }

        //#####################################################################################

        [Authorize]
        public async Task<IActionResult> BookingToogle(Guid? id)
        {
            if (id == null) return RedirectToAction(nameof(Index)); //ToDo: Send a "explonaton" to the View.

            var userId = _userManager.GetUserId(User);
            if (userId == null) return RedirectToAction(nameof(Index)); //ToDo: Send a "explonaton" to the View.

            var attending = await _context.ApplicationUserGymClass
                .FirstOrDefaultAsync(a => a.applicationUserId.Equals(userId) && a.gymClassId == id);

            if (attending != null)
            {
                _context.Remove(attending);
            }
            else
            {
                var bookin = new ApplicationUserGymClass
                {
                    applicationUserId = userId,
                    gymClassId = (Guid)id
                };
                _context.ApplicationUserGymClass.Add(bookin);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
