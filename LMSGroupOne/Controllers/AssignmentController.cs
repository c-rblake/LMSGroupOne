using LMS.Core.Models.Entities;
using LMS.Core.Models.ViewModels.Assignment;
using LMS.Data.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSGroupOne.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Person> _userManager;

        public AssignmentController(ApplicationDbContext db, UserManager<Person> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var modules = await _db.Persons
                .Where(p => p.Id == userId)
                .Select(p => p.Course)
                .Select(c => c.Modules)
                .FirstOrDefaultAsync();

            var activities = modules
                .Select(a => a.Activities
                    .Where(t => t.ActivityType.Name == "Assignment"))
                .FirstOrDefault();

            var viewModels = new List<AssignmentViewModel>();

            foreach (var activity in activities)
            {
                var viewModel = new AssignmentViewModel
                {
                    ActivityId = activity.Id,
                    ActivityName = activity.Name,
                    StartDate = activity.StartDate,
                    EndDate = activity.EndDate,
                    IsFinished = GetIsFinished(activity.Id, userId),
                    IsLate = GetIsLate(activity.Id, userId, activity.EndDate)
                };
                viewModels.Add(viewModel);
            }

            return View(viewModels);
        }
        
        [Authorize]
        [Route("/assignment/details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);

            var documentTimeStamp = await _db.Documents
                                            .Where(d => d.Id == id)
                                            .Where(d => d.PersonId == userId)
                                            .Select(t => t.TimeStamp)
                                            .FirstOrDefaultAsync();

            var activity = await _db.Activities
                                .Where(a => a.Id == id)
                                .FirstOrDefaultAsync();

            var module = await _db.Modules
                                .Where(a => a.Id == activity.ModuleId)
                                .FirstOrDefaultAsync();

            var viewModel = new AssignmentDetailsViewModel
            {
                ActivityId = activity.Id,
                ActivityName = activity.Name,
                ActivityDescription = activity.Description,
                Module = module,
                StartDate = activity.StartDate,
                EndDate = activity.EndDate,
                IsFinished = GetIsFinished(activity.Id, userId),
                IsLate = GetIsLate(activity.Id, userId, activity.EndDate)
            };

            if (viewModel.IsFinished == true)
            {
                viewModel.DocumentTimeStamp = documentTimeStamp;
            };

            return View(viewModel);

        }

        public bool GetIsLate(int id, string userId, DateTime endDate)
        {
            return _db.Documents
                    .Where(a => a.ActivityId == id)
                    .Where(p => p.PersonId == userId)
                    .Any(u => u.TimeStamp > endDate);
        }

        public bool GetIsFinished(int id, string userId)
        {
            return _db.Documents
                    .Where(a => a.ActivityId == id)
                    .Any(p => p.PersonId == userId);
        }
    }
}