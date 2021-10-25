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

            return PartialView();
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateModule(CreateModuleViewModel createdModule)
        {
            int courseId = createdModule.CourseId;
            IEnumerable<Module> modules = await GetAllModulesByCourseAsync(courseId);

            foreach (Module existingModule in modules)
            {
                if (createdModule.StartDate <= existingModule.StartDate && createdModule.EndDate >= existingModule.StartDate)
                {
                    ModelState.AddModelError("Name", "This Module overlaps with current Modules");

                    return PartialView(createdModule);
                }
            }

            if (ModelState.IsValid)
            {
                uow.ModuleRepository.AddModule(mapper.Map<Module>(createdModule));
                await uow.CompleteAsync();
            }

            var courses = await uow.CourseRepository.GetAsync();
            ViewBag.Courses = courses;

            return PartialView(createdModule);
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

            return PartialView(module);
        }

        [HttpPost]
        [Route("/module/edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditModule(int id, EditModuleViewModel editedModule)
        {
            if (id != editedModule.Id)
            {
                return NotFound();
            }

            // Verify that Dates on this Module don't start earlier or end later than its Course
            var course = await uow.CourseRepository.GetCourse(id);

            if (editedModule.StartDate < course.StartDate || editedModule.EndDate > course.EndDate)
            {
                ModelState.AddModelError("Name", $"Please keep dates within Course Dates ({course.StartDate}-{course.EndDate})");
                return PartialView(editedModule);
            }

            // Get all modules on course except this one being edited
            IEnumerable<Module> modules = await GetAllModulesByCourseAsync(course.Id);
            modules = modules.Where(a => a.Id != id);

            // Verify Module Dates to existing Module Dates
            foreach (Module existingModule in modules)
            {
                if (editedModule.StartDate <= existingModule.StartDate && editedModule.EndDate > existingModule.StartDate)
                {
                    String moduleWithDates = $"Module {existingModule.Name} ({existingModule.StartDate.ToString("yyyy-MM-dd")} - {existingModule.EndDate.ToString("yyyy-MM-dd")})";
                    ModelState.AddModelError("Description", $"1 This module overlaps dates with {moduleWithDates}");
                }

                var entity = await uow.ModuleRepository.FindAsync(id);
                ViewBag.moduleName = entity.Name;
                return PartialView(editedModule);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var module = await uow.ModuleRepository.FindAsync(id);
                    mapper.Map(editedModule, module);
                    await uow.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!uow.ModuleRepository.ModuleExistsById(editedModule.Id))
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
            return PartialView(editedModule);
        }

        private async Task<IEnumerable<Module>> GetAllModulesByCourseAsync(int courseId)
        {
            return await uow.ModuleRepository.GetModulesByCourseId(courseId);
        }

    }

 }
