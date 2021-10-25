using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Models.ViewModels.Assignment
{
    public class AssignmentViewModel
    {
        public int ActivityId { get; set; }
        [Display(Name = "Assignment")]
        public string ActivityName { get; set; }
        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End date")]
        public DateTime EndDate { get; set; }
        public bool IsFinished { get; set; }
        public bool IsLate { get; set; }
    }
}