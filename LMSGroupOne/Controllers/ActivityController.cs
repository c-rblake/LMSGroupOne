using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LMS.Core.Models.Entities;
using LMS.Core.Models.ViewModels.Activity;
using LMS.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

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
    }
}
