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
            return PartialView();
        }
        [Route("/activity/create/moduleid/{moduleid}")]
        public async Task<IActionResult> Create(int moduleid)
        {
            var activityTypes = await GetActivityTypes();
            var module = await uow.ModuleRepository.GetModule(moduleid);
            ViewBag.activityTypes = activityTypes;
            ViewBag.module = $"{module.Name} {module.StartDate.ToString("yyyy-MM-dd")}--{module.EndDate.Date.ToString("yyyy-MM-dd")}";
            return PartialView(new ActivityCreateViewModel());
        }

        [HttpPost]
        [Route("/activity/create/moduleid/{moduleid}")]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int moduleid,ActivityCreateViewModel model)
        {
           // int moduleId = model.ModuleId;
            IEnumerable<Activity> activities = await GetAllActivitiesByModuleAsync(moduleid);
            var activityTypes = await GetActivityTypes();
            //model.ActivityTypeId = ActivityTypeId;
            //var modules = await GetModules();
            var module = await uow.ModuleRepository.GetModule(moduleid);
            ViewBag.activityTypes = activityTypes;
            ViewBag.module = $"{module.Name} {module.StartDate}--{module.EndDate}" ;
            if (model.StartDate < module.StartDate || model.EndDate > module.EndDate)
            {
                ModelState.AddModelError("Name", $"one part of this activity is outside the period for {module.Name}.");
                return View(model);
            }
            
            foreach (Activity activity in activities)
            {
                if(model.StartDate<=activity.EndDate && model.EndDate >= activity.StartDate)
                {
                    ModelState.AddModelError("Name", "this activity overlaps with current activities");
                    
                    return View(model);
                }
            }
            //model.Success = true;
            //model.Message = "activity was created";
           
            if (ModelState.IsValid)
            {
                uow.ActivityRepository.AddActivity(mapper.Map<Activity>(model));
                await uow.CompleteAsync();
              //  model.ReturnId = uow.ActivityRepository.GetActivityId(model.Name);
            }
            return PartialView(model);
        }

        [Route("/activity/edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = mapper.Map<ActivityEditViewModel>(await uow.ActivityRepository.FindAsync(id));
            if (course == null)
            {
                return NotFound();
            }
            var activityTypes = await GetActivityTypes();
            var modules = await GetModules();
            ViewBag.activityTypes = activityTypes;
            ViewBag.modules = modules;
            return View(course);
        }

        [HttpPost]
        [Route("/activity/edit/{id}")]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ActivityEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }
            int moduleId = viewModel.ModuleId;
            IEnumerable<Activity> activities = await GetAllActivitiesByModuleAsync(moduleId);
            var activityTypes = await GetActivityTypes();
            var modules = await GetModules();
            ViewBag.activityTypes = activityTypes;
            ViewBag.modules = modules;
            foreach (Activity activity in activities)
            {
                if (viewModel.StartDate <= activity.EndDate && viewModel.EndDate >= activity.StartDate)
                {
                    ModelState.AddModelError("Name", "this activity overlaps with current activities");
                   
                    return View(viewModel);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var course = await uow.ActivityRepository.FindAsync(id);
                    mapper.Map(viewModel, course);
                    await uow.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!uow.ActivityRepository.ActivityExistsById(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(viewModel);
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
