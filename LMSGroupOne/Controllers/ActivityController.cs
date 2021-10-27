using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LMS.Core.Models.Entities;
using LMS.Core.Models.ViewModels.Activity;
using LMS.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMSGroupOne.Controllers
{
    public class ActivityController : Controller
    {

        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public ActivityController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(int id)
        {
            var activityTypes = await GetActivityTypes();
            ViewBag.activityTypes = activityTypes;
            var module = await uow.ModuleRepository.GetModule(id);
            ViewBag.module = $"{module.Name} {module.StartDate.ToString("yyyy-MM-dd")}--{module.EndDate.Date.ToString("yyyy-MM-dd")}";

            var model = new ActivityCreateViewModel
            {
                ModuleId = id,
                StartDate = DateTime.Now,
                EndDate=DateTime.Now,

                Message = "",
                ReturnId = 0,
                Success = false,
            };


            return PartialView(model);
        }

        [HttpPost]
        //[Route("/activity/create")]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActivityCreateViewModel model)
        {
            int moduleId = model.ModuleId;
            IEnumerable<Activity> activities = await GetAllActivitiesByModuleAsync(moduleId);
            var activityTypes = await GetActivityTypes();
            var modules = await GetModules();
            ViewBag.activityTypes = activityTypes;
            var module = await uow.ModuleRepository.GetModule(moduleId);
            ViewBag.module = $"{module.Name} {module.StartDate}--{module.EndDate}";
            if (model.StartDate < module.StartDate || model.EndDate > module.EndDate)
            {
                ModelState.AddModelError("Name", $"one part of this activity is outside the period for {module.Name}.");
            }
            foreach (Activity activity in activities)
            {
                if(model.StartDate<=activity.EndDate && model.EndDate >= activity.StartDate)
                {
                    ModelState.AddModelError("Name", "this activity overlaps with current activities");
                    model.Success = false;
                    break;
                }
            }
            if (ModelState.IsValid)
            {
                Activity activity = mapper.Map<Activity>(model);
                uow.ActivityRepository.AddActivity(activity);
                uow.CompleteAsync().Wait();  // task need to finish before extracting id
                model.ReturnId = activity.Id;
                model.Success = true;
                model.Message = "Activity was created!";

            }
            return PartialView(model);
        }

        //[Route("/activity/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {


            var model = mapper.Map<ActivityEditViewModel>(await uow.ActivityRepository.FindAsync(id));
            if (model == null)
            {
                model = new ActivityEditViewModel();
                model.Success = false;
                model.ReturnId = 0;
                model.Message = "Activity not found!";
                model.Id = id;
                return PartialView(model);
            }
            var module = await uow.ModuleRepository.GetModule(model.ModuleId);
            ViewBag.module = $"{module.Name} {module.StartDate.ToString("yyyy-MM-dd")}--{module.EndDate.Date.ToString("yyyy-MM-dd")}";
            model.Id = id;
            model.Message = "";
            model.Success = false;
            model.ReturnId = 0;


            var activityTypes = await GetActivityTypes();
            ViewBag.activityTypes = activityTypes;
            return PartialView(model);
        }

        [HttpPost]
        //[Route("/activity/edit/{id}")]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ActivityEditViewModel viewModel)
        {
            //if (id != viewModel.Id)
            //{
            //    return NotFound();
            //}
            int moduleId = viewModel.ModuleId;
            IEnumerable<Activity> activities = await GetAllActivitiesByModuleAsync(moduleId);
            
            var activityTypes = await GetActivityTypes();
            ViewBag.activityTypes = activityTypes;
            var module = await uow.ModuleRepository.GetModule(moduleId);
            ViewBag.module = $"{module.Name} {module.StartDate.ToString("yyyy-MM-dd")}--{module.EndDate.Date.ToString("yyyy-MM-dd")}";
            if (viewModel.StartDate < module.StartDate || viewModel.EndDate > module.EndDate)
            {
                ModelState.AddModelError("Name", $"one part of this activity is outside {module.Name} period");
            }

            activities = activities.Where(a => a.Id != viewModel.Id);
            foreach (Activity activity in activities) 
            {
                if (viewModel.StartDate <= activity.EndDate && viewModel.EndDate >= activity.StartDate)
                {
                    ModelState.AddModelError("Name", "this activity overlaps with current activities");
                    break;
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var activity = await uow.ActivityRepository.FindAsync(viewModel.Id);
                    mapper.Map(viewModel, activity);
                    await uow.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!uow.ActivityRepository.ActivityExistsById(viewModel.Id))
                    {
                        viewModel.Success = false;
                        viewModel.Message = "Activity not found!";
                        return PartialView(viewModel);
                    }
                    else
                    {
                        throw;
                    }
                    viewModel.Success = false;
                    viewModel.Message = "DatabaseExeption Exeption!";
                    return PartialView(viewModel);
                }

                viewModel.Success = true;
                viewModel.Message = "Activity edited!";
                viewModel.ReturnId = viewModel.Id;
                return PartialView(viewModel);
            }
            return PartialView(viewModel);
        }

        private async Task<IEnumerable<Activity>> GetAllActivitiesByModuleAsync(int moduleId)
        {
            return await uow.ActivityRepository.GetActivitiesByModuleId(moduleId);
        }
        private async Task<IEnumerable<ActivityType>> GetActivityTypes()
        {
            return await uow.ActivityTypeRepository.GetAsync();
        }
        private async Task<IEnumerable<Module>> GetModules()
        {
            return await uow.ModuleRepository.GetAsync();
        }

    }
}
