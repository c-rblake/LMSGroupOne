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

            var viewModels = await _db.Persons
                .Where(p => p.Id == userId)
                .Include(p => p.Course)
                .ThenInclude(c => c.Modules)
                .ThenInclude(m => m.Activities)
                .ThenInclude(a => a.ActivityType)
                .Select(p => p.Course)
                .SelectMany(c => c.Modules)
                .SelectMany(m => m.Activities)
                .Where(a => a.ActivityType.Name == "Assignment")
                .Select(activity => new AssignmentViewModel
                    {
                        ActivityId = activity.Id,
                        ActivityName = activity.Name,
                        StartDate = activity.StartDate,
                        EndDate = activity.EndDate,
                        IsFinished = activity.Documents.FirstOrDefault(d => d.PersonId == userId) != null ? true : false ,
                        IsLate = activity.Documents.FirstOrDefault(d => d.PersonId == userId).TimeStamp > activity.EndDate ? true : false
                })
                .ToListAsync();

            /*var viewModels = activities
                                .Where(a => a.ActivityType.Name == "Assignment")
                                .Select(activity => new AssignmentViewModel
                                {
                                    ActivityId = activity.Id,
                                    ActivityName = activity.Name,
                                    StartDate = activity.StartDate,
                                    EndDate = activity.EndDate,
                                    IsFinished = GetIsFinished(activity.Id, userId),
                                    IsLate = GetIsLate(activity.Id, userId, activity.EndDate)
                                })
                                .ToList();*/

            return View(viewModels);

        }
        
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);
          
            var viewModel = await _db.Activities
                                    .Select(a => new AssignmentDetailsViewModel
                                    {
                                        ActivityId = a.Id,
                                        ActivityName = a.Name,
                                        ActivityDescription = a.Description,
                                        Module = a.Module,
                                        StartDate = a.StartDate,
                                        EndDate = a.EndDate,
                                        DocumentTimeStamp = a.Documents.FirstOrDefault(d => d.PersonId == userId).TimeStamp,
                                    })
                                    .FirstOrDefaultAsync(a => a.ActivityId == id);

            viewModel.IsFinished = viewModel.DocumentTimeStamp != null ? true : false;

            viewModel.IsLate = viewModel.DocumentTimeStamp > viewModel.EndDate ? true : false;

            return View(viewModel);

        }
    }
}