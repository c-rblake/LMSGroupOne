using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LMS.Core.Models.Entities;
using LMS.Core.Models.ViewModels.Activity;
using LMS.Core.Repositories;
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

        public async Task<IActionResult> Create()
        {
            var activityTypes = await uow.ActivityTypeRepository.GetAsync();
            var modules = await uow.ModuleRepository.GetAsync();
            ViewBag.activityTypes = activityTypes;
            ViewBag.modules = modules;
            return View();
        }

        [HttpPost]
        [Route("/activity/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActivityCreateViewModel activity)
        {
            if (ModelState.IsValid)
            {
                uow.ActivityRepository.AddActivity(mapper.Map<Activity>(activity));
                await uow.CompleteAsync();
            }

            return RedirectToAction("Index","Home");
        }

        public IActionResult VerifyActivityName(string Name)
        {
            bool courseExists = uow.ActivityRepository.ActivityExists(Name);
            if (courseExists)
            {
                return Json($"An Activity with name {Name} already exists.");
            }
            return Json(true);
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
            var activityTypes = await uow.ActivityTypeRepository.GetAsync();
            var modules = await uow.ModuleRepository.GetAsync();
            ViewBag.activityTypes = activityTypes;
            ViewBag.modules = modules;
            return View(course);
        }

        [HttpPost]
        [Route("/activity/edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ActivityEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
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

    }
}
