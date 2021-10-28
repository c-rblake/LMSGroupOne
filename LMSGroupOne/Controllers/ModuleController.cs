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

            // Verify Dates
            Task<string> dateCheckMessage = areModuleDatesValid(course, createdModule.StartDate, createdModule.EndDate, createdModule.Id);

            if (dateCheckMessage.Result != "")
            {
                ModelState.AddModelError("Name", dateCheckMessage.Result);
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
            var module = await uow.ModuleRepository.GetModule(id);
            var course = await uow.CourseRepository.GetCourse(module.CourseId);
            ViewBag.courseName = $"{course.Name}";
            ViewBag.courseDates = $"{ course.StartDate.ToString("yyyy-MM-dd")} - { course.EndDate?.Date.ToString("yyyy-MM-dd")}";

            var moduleViewModel = mapper.Map<EditModuleViewModel>(await uow.ModuleRepository.FindAsync(id));
            if (moduleViewModel == null)
            {
                return NotFound();
            }

            return PartialView(moduleViewModel);
        }

        // Edit Module POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditModule(EditModuleViewModel editedModule)
        {
            // Get Course Name + Dates to display on Module Form to make it easier for user to set Module Dates
            var course = await uow.CourseRepository.GetCourse(editedModule.CourseId);
            ViewBag.courseName = $"{course.Name}";
            ViewBag.courseDates = $"{ course.StartDate.ToString("yyyy-MM-dd")} - { course.EndDate?.Date.ToString("yyyy-MM-dd")}";

            // Verify Dates
            Task<string> dateCheckMessage = areModuleDatesValid(course, editedModule.StartDate, editedModule.EndDate, editedModule.Id);

            if (dateCheckMessage.Result != "")
            {
                ModelState.AddModelError("Name", dateCheckMessage.Result);
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

        /*
        #################################################
                DATE VALIDATION
        #################################################

        Module är den modul man skapar eller editerar.
        ExistingModule är andra moduler på samma kurs.
        Fel uppstår om:

        Test av Modulens egna Start/Slutdatum (Görs via Annotation i Model, Se LMS.Core.Validations.CheckModuleDates)
         - Module.StartDate > Module.EndDate

        Test av Module mot Course:
         - Module.StartDate < Course.StartDate
         - Module.EndDate > Course.EndDate

        Test av Module mot ExistingModule på samma Course:
         - Module.StartDate < ExistingModule.EndDate && Module.EndDate > ExistingModule.StartDate
         - Module.EndDate > ExistingModule.StartDate && Module.EndDate < ExistingModule.StartDate
        
         */

        private async Task<string> areModuleDatesValid(Course course, DateTime moduleStartDate, DateTime moduleEndDate, int moduleId)
        {
            // Verify Modules dates vs Courses dates
            if (moduleStartDate.Date < course.StartDate.Date)
            {
                return "This Modules Start Date is earlier than the Course Start Date";
            }

            if (moduleEndDate.Date > course.EndDate?.Date)
            {
                return "This Modules End Date is later than the Course End Date";
            }

            // Get all modules on course except this one being edited
            IEnumerable<Module> modules = await GetAllModulesByCourseAsync(course.Id);
            modules = modules.Where(a => a.Id != moduleId);

            // Verify Module Dates to existing Module Dates
            foreach (Module existingModule in modules)
            {
                if (moduleStartDate.Date < existingModule.EndDate.Date && moduleEndDate.Date > existingModule.StartDate.Date)
                {
                    String moduleWithDates = $"Module {existingModule.Name} ({existingModule.StartDate.ToString("yyyy-MM-dd")} - {existingModule.EndDate.ToString("yyyy-MM-dd")})";
                    return $"This modules dates overlaps existing {moduleWithDates}";
                }

                if (moduleEndDate.Date > existingModule.StartDate.Date && moduleStartDate < existingModule.EndDate.Date)
                {
                    String moduleWithDates = $"Module {existingModule.Name} ({existingModule.StartDate.ToString("yyyy-MM-dd")} - {existingModule.EndDate.ToString("yyyy-MM-dd")})";
                    return $"This modules End Date is later than the Start Date of existing {moduleWithDates}";
                }

            }

            return "";

        }

        private async Task<IEnumerable<Module>> GetAllModulesByCourseAsync(int courseId)
        {
            return await uow.ModuleRepository.GetModulesByCourseId(courseId);
        }

    }

 }
