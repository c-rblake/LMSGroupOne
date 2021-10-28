using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LMS.Core.Models.ViewModels.Module;

namespace LMSGroupOne.Validations
{
    public class CheckModuleDates : ValidationAttribute
    {
        CreateModuleViewModel createModel;
        EditModuleViewModel editModel;
        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            const string errorMessage = "EndDate must come after StartDate";
            if (value is DateTime input)
            {
                if (validationContext.ObjectInstance is CreateModuleViewModel)
                    createModel = (CreateModuleViewModel)validationContext.ObjectInstance;
                else if (validationContext.ObjectInstance is EditModuleViewModel)
                    editModel = (EditModuleViewModel)validationContext.ObjectInstance;
                if (createModel != null)
                {
                    if (createModel.EndDate > createModel.StartDate)
                        return ValidationResult.Success;
                    else
                        return new ValidationResult(errorMessage);
                }
                else if (editModel != null)
                {
                    if (editModel.EndDate > editModel.StartDate)
                        return ValidationResult.Success;
                    else
                        return new ValidationResult(errorMessage);
                }
            }
            return new ValidationResult(errorMessage);
        }
    }
}