using LMS.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Models.ViewModels.Assignment
{
    public class AssignmentIndexTeacherViewModel
    {
        [Display(Name = "Assignment")]
        public string ActivityName { get; set; }
        [Display(Name = "Start date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ActivityStartDate { get; set; }
        [Display(Name = "Due date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ActivityEndDate { get; set; }
        [Display(Name = "Description")]
        public string ActivityDescription { get; set; }
        [Display(Name = "Assignment document name ")]
        public string DocumentName { get; set; }
        [Display(Name = "Module")]
        public string ModuleName { get; set; }
        [Display(Name = "Course")]
        public string CourseName { get; set; }
        [Display(Name = "Student")]
        public string PersonName { get; set; }
        [Display(Name = "Date submitted")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime? DocumentTimeStamp { get; set; }
        [Display(Name = "Submitted")]
        public bool IsFinished { get; set; }
        [Display(Name = "Late")]
        public bool IsLate { get; set; }
    }
}