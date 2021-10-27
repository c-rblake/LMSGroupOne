using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LMS.Core.Models.ViewModels.Activity;
using LMS.Core.Models.ViewModels.Course;

namespace LMSGroupOne.Validations
{
    public class CheckDates : ValidationAttribute
    {
        ActivityCreateViewModel activityCreate;
        ActivityEditViewModel activityEdit;
        CourseEditViewModel courseEdit;
        CreateCourseViewModel courseCreate;
        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            const string errorMessage = "EndDate must come after StartDate";
            if (value is DateTime input)
            {
                if (validationContext.ObjectInstance is ActivityCreateViewModel)
                    activityCreate = (ActivityCreateViewModel)validationContext.ObjectInstance;
                else if (validationContext.ObjectInstance is ActivityEditViewModel)
                    activityEdit = (ActivityEditViewModel)validationContext.ObjectInstance;
                else if (validationContext.ObjectInstance is CreateCourseViewModel)
                    courseCreate = (CreateCourseViewModel)validationContext.ObjectInstance;
                else if (validationContext.ObjectInstance is CourseEditViewModel)
                    courseEdit = (CourseEditViewModel)validationContext.ObjectInstance;
                if (activityCreate != null)
                {
                    if (activityCreate.EndDate > activityCreate.StartDate) return ValidationResult.Success;
                    else return new ValidationResult(errorMessage);
                }
                else if (activityEdit != null)
                {
                    if (activityEdit.EndDate > activityEdit.StartDate) return ValidationResult.Success;
                    else return new ValidationResult(errorMessage);
                }
                else if (courseCreate != null)
                {
                    if (courseCreate.EndDate > courseCreate.StartDate) return ValidationResult.Success;
                    else return new ValidationResult(errorMessage);
                }

                else if (courseEdit != null)
                {
                    if (courseEdit.EndDate > courseEdit.StartDate) return ValidationResult.Success;
                    else return new ValidationResult(errorMessage);
                }
            }
            return new ValidationResult(errorMessage);
        }
    }
}