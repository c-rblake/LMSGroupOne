using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LMS.Core.Models.Entities;
using LMS.Core.Models.ViewModels.Module;
using LMS.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMSGroupOne.Controllers
{
    public class ModuleController : Controller
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public ModuleController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateModule()
        {
            // Temporary for testing, IRL this will populate from the Course you're creating the Module from
            var courses = await uow.CourseRepository.GetAsync();
            ViewBag.Courses = courses;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateModule(CreateModuleViewModel model)
        {
            int courseId = model.CourseId;
            IEnumerable<Module> modules = await GetAllModulesByCourseAsync(courseId);
           
            foreach (Module module in modules)
            {
                if (model.StartDate <= module.EndDate && model.EndDate >= module.StartDate)
                {
                    ModelState.AddModelError("Name", "This Module overlaps with current Modules");

                    return View(model);
                }
            }

            if (ModelState.IsValid)
            {
                uow.ModuleRepository.AddModule(mapper.Map<Module>(model));
                await uow.CompleteAsync();
            }

            var courses = await uow.CourseRepository.GetAsync();
            ViewBag.Courses = courses;

            return View(model);
        }

        public IActionResult VerifyModuleName(string Name)
        {
            bool moduleExists = uow.ModuleRepository.ModuleExists(Name);
            if (moduleExists)
            {
                return Json($"A Module with name {Name} already exists.");
            }
            return Json(true);
        }



        [Route("/module/edit/{id}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> EditModule(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var module = mapper.Map<EditModuleViewModel>(await uow.ModuleRepository.FindAsync(id));
            if (module == null)
            {
                return NotFound();
            }
            
            var modules = await uow.ModuleRepository.GetAsync();
            ViewBag.modules = modules;

            return View(module);
        }

        [HttpPost]
        [Route("/module/edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditModule(int id, EditModuleViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var module = await uow.ModuleRepository.FindAsync(id);
                    mapper.Map(viewModel, module);
                    await uow.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!uow.ModuleRepository.ModuleExistsById(viewModel.Id))
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


        private async Task<IEnumerable<Activity>> GetAllModulesByCourseAsync(int courseId)
        {
            return await uow.ModuleRepository.GetModulesByCourseId(courseId);
        }

    }
}
