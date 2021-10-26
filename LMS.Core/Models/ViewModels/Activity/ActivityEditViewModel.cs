using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMSGroupOne.Validations;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Core.Models.ViewModels.Activity
{
    public class ActivityEditViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int ActivityTypeId { get; set; }
        public int ModuleId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        [CheckDates]
        public DateTime EndDate { get; set; }
    }
}
