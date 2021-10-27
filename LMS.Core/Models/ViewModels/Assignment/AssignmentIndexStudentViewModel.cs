using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Models.ViewModels.Assignment
{
    public class AssignmentIndexStudentViewModel
    {
        public int ActivityId { get; set; }
        [Display(Name = "Assignment")]
        public string ActivityName { get; set; }
        [Display(Name = "Start date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Due date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime EndDate { get; set; }
        public bool IsFinished { get; set; }
        public bool IsLate { get; set; }
    }
}