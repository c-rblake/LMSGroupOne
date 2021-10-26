using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMSGroupOne.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMS.Core.Models.ViewModels.Activity
{
    public class ActivityCreateViewModel: IModalViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int ActivityTypeId { get; set; }
        public int ModuleId { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Required]
        [CheckDates]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        

        public bool Success { get; set; }  // creation status
        public string Message { get; set; }   // returnmessage
        public int ReturnId { get; set; }    // return id when created
    }
}
