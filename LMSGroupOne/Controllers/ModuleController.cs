using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        // Create Module GET
        public async Task<IActionResult> CreateModule(int id)
        {
            // Get Course Name + Dates to display on Module Form to make it easier for user to set Module Dates
            var course = await uow.CourseRepository.GetCourse(id);
            ViewBag.courseName = $"{course.Name}";
            ViewBag.courseDates = $"{ course.StartDate.ToString("yyyy-MM-dd")} - { course.EndDate?.Date.ToString("yyyy-MM-dd")}";
           
            var model = new CreateModuleViewModel
            {
                
                CourseId = id,
                
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                
                Message = "",
                ReturnId = 0,
                Success = false,

            };


            return PartialView(model);
        }

        // Create Module POST
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateModule(CreateModuleViewModel createdModule)
        {
            // Get Course Name + Dates to display on Module Form to make it easier for user to set Module Dates
            var course = await uow.CourseRepository.GetCourse(createdModule.CourseId);
            ViewBag.courseName = $"{course.Name}";
            ViewBag.courseDates = $"{ course.StartDate.ToString("yyyy-MM-dd")} - { course.EndDate?.Date.ToString("yyyy-MM-dd")}";

            // Verify Modules dates vs Courses dates
            if (createdModule.StartDate.Date < course.StartDate.Date)
            {
                ModelState.AddModelError("StartDate", $"This Modules Start Date is earlier than the Course Start Date");
                return PartialView(createdModule);
            }

            if (createdModule.EndDate.Date > course.EndDate?.Date)
            {
                ModelState.AddModelError("EndDate", $"This Modules End Date is later than the Course End Date");
                return PartialView(createdModule);
            }

            // Get all modules on course except this one being edited
            IEnumerable<Module> modules = await GetAllModulesByCourseAsync(course.Id);
            modules = modules.Where(a => a.Id != createdModule.Id);

            // Verify Module Dates to existing Module Dates
            foreach (Module existingModule in modules)
            {
                if (createdModule.StartDate.Date < existingModule.EndDate.Date && createdModule.EndDate.Date > existingModule.StartDate.Date)
                {
                    String moduleWithDates = $"Module {existingModule.Name} ({existingModule.StartDate.ToString("yyyy-MM-dd")} - {existingModule.EndDate.ToString("yyyy-MM-dd")})";
                    ModelState.AddModelError("StartDate", $"This modules dates overlaps existing {moduleWithDates}");
                    return PartialView(createdModule);
                }
     
                if (createdModule.EndDate.Date > existingModule.StartDate.Date)
                {
                    String moduleWithDates = $"Module {existingModule.Name} ({existingModule.StartDate.ToString("yyyy-MM-dd")} - {existingModule.EndDate.ToString("yyyy-MM-dd")})";
                    ModelState.AddModelError("EndDate", $"This modules End Date is later than the Start Date of existing {moduleWithDates}");
                    return PartialView(createdModule);
                }

            }

            if (ModelState.IsValid)
            {
                Module mod = mapper.Map<Module>(createdModule);
                uow.ModuleRepository.AddModule(mod);
                await uow.CompleteAsync();
                createdModule.Success = true;
                createdModule.ReturnId = mod.Id;
                createdModule.Message = "Module created";

            }
            else
            {
                createdModule.Success = false;
                createdModule.Message = "Could not create Module!";
            }

            //var courses = await uow.CourseRepository.GetAsync();
            //ViewBag.Courses = courses;

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


        // Edit Module GET
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> EditModule(int id)
        {
            // Get Course Name + Dates to display on Module Form to make it easier for user to set Module Dates
            var course = await uow.CourseRepository.GetCourse(id);
            ViewBag.courseName = $"{course.Name}";
            ViewBag.courseDates = $"{ course.StartDate.ToString("yyyy-MM-dd")} - { course.EndDate?.Date.ToString("yyyy-MM-dd")}";

            var module = mapper.Map<EditModuleViewModel>(await uow.ModuleRepository.FindAsync(id));
            if (module == null)
            {
                return NotFound();
            }

            return PartialView(module);
        }

        // Edit Module POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditModule(EditModuleViewModel editedModule)
        {
            // Get Course Name + Dates to display on Module Form to make it easier for user to set Module Dates
            var course = await uow.CourseRepository.GetCourse(editedModule.Id);
            ViewBag.courseName = $"{course.Name}";
            ViewBag.courseDates = $"{ course.StartDate.ToString("yyyy-MM-dd")} - { course.EndDate?.Date.ToString("yyyy-MM-dd")}";

            // Verify Modules dates vs Courses dates
            if (editedModule.StartDate.Date < course.StartDate.Date)
            {
                ModelState.AddModelError("StartDate", $"This Modules Start Date is earlier than the Course Start Date");
                return PartialView(editedModule);
            }

            if (editedModule.EndDate.Date > course.EndDate?.Date)
            {
                ModelState.AddModelError("EndDate", $"This Modules End Date is later than the Course End Date");
                return PartialView(editedModule);
            }

            // Get all modules on course except this one being edited
            IEnumerable<Module> modules = await GetAllModulesByCourseAsync(course.Id);
            modules = modules.Where(a => a.Id != editedModule.Id);

            // Verify Module Dates to existing Module Dates
            foreach (Module existingModule in modules)
            {
                if (editedModule.StartDate.Date < existingModule.EndDate.Date && editedModule.EndDate.Date > existingModule.StartDate.Date)
                {
                    String moduleWithDates = $"Module {existingModule.Name} ({existingModule.StartDate.ToString("yyyy-MM-dd")} - {existingModule.EndDate.ToString("yyyy-MM-dd")})";
                    ModelState.AddModelError("StartDate", $"This modules dates overlaps existing {moduleWithDates}");
                    return PartialView(editedModule);
                }

                if (editedModule.EndDate.Date > existingModule.StartDate.Date)
                {
                    String moduleWithDates = $"Module {existingModule.Name} ({existingModule.StartDate.ToString("yyyy-MM-dd")} - {existingModule.EndDate.ToString("yyyy-MM-dd")})";
                    ModelState.AddModelError("EndDate", $"This modules End Date is later than the Start Date of existing {moduleWithDates}");
                    return PartialView(editedModule);
                }

            }

            if (ModelState.IsValid)
            {
                try
                {
                    var module = await uow.ModuleRepository.FindAsync(editedModule.Id);
                    mapper.Map(editedModule, module);
                    await uow.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {                   
                    
                    editedModule.Message = "Failed to edit module!";
                    editedModule.Success = false;
                    
                    if (!uow.ModuleRepository.ModuleExistsById(editedModule.Id))
                    {
                        editedModule.Message = "Module Not found!";
                    }

                    return PartialView(editedModule);
                }
                editedModule.Message = "Module edited!";
                editedModule.Success = true;
                editedModule.ReturnId = editedModule.Id;
                return PartialView(editedModule);

            }
            editedModule.Message = "Module not edited!";
            editedModule.Success = false;
            editedModule.ReturnId = editedModule.Id;
            return PartialView(editedModule);
        }

        private async Task<IEnumerable<Module>> GetAllModulesByCourseAsync(int courseId)
        {
            return await uow.ModuleRepository.GetModulesByCourseId(courseId);
        }

    }

 }
