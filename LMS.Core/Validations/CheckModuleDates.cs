using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Core.Models.Entities;
using LMS.Core.Models.ViewModels.Module;

namespace LMSGroupOne.Validations
{
    class CheckModuleDates : ValidationAttribute
    {
        CreateModuleViewModel moduleCreate;
        EditModuleViewModel moduleEdit;

        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            int CourseId = 0;
            int ModuleId = 0;
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;
            string errorMessage = "";
            
           

            // Determine which ViewModel to use as Source
            if (value is DateTime input)
            {
                if (validationContext.ObjectInstance is CreateModuleViewModel)
                {
                    moduleCreate = (CreateModuleViewModel)validationContext.ObjectInstance;
                    CourseId = moduleCreate.CourseId;
                    ModuleId = moduleCreate.Id;
                    StartDate = moduleCreate.StartDate;
                    EndDate = moduleCreate.EndDate;
                }
                    
                else if (validationContext.ObjectInstance is EditModuleViewModel)
                {
                    moduleEdit = (EditModuleViewModel)validationContext.ObjectInstance;
                    CourseId = moduleEdit.CourseId;
                    ModuleId = moduleEdit.Id;
                    StartDate = moduleEdit.StartDate;
                    EndDate = moduleEdit.EndDate;
                }
                    
                                
                // Verify that Dates on this Module don't start earlier or end later than its Course
                var course = await uow.CourseRepository.GetCourse(CourseId);
                
                if (StartDate < course.StartDate || EndDate > course.EndDate)
                {
                    errorMessage = $"Please keep dates within Course Dates ({course.StartDate.ToString("yyyy-MM-dd")}-{course.EndDate?.ToString("yyyy-MM-dd")})";
                }


                // Get all modules on course except this one being edited
                IEnumerable<Module> modules = await GetAllModulesByCourseAsync(CourseId);
                modules = modules.Where(a => a.Id != ModuleId);

                // Verify Module Dates to existing Module Dates
                foreach (Module existingModule in modules)
                {
                    if (createdModule.StartDate <= existingModule.StartDate && createdModule.EndDate > existingModule.StartDate)
                    {
                        String moduleWithDates = $"Module {existingModule.Name} ({existingModule.StartDate.ToString("yyyy-MM-dd")} - {existingModule.EndDate.ToString("yyyy-MM-dd")})";
                        ModelState.AddModelError("Description", $"1 This module overlaps dates with {moduleWithDates}");
                    }

                    var entity = await uow.ModuleRepository.FindAsync(createdModule.Id);
                    ViewBag.moduleName = entity.Name;
                    return PartialView(createdModule);
                }

            }

            

            return new ValidationResult(errorMessage);
        }
    }
}
