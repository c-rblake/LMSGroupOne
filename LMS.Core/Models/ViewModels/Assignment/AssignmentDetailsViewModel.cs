using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Models.ViewModels.Assignment
{
    public class AssignmentDetailsViewModel
    {
        public int ActivityId { get; set; }
        [Display(Name = "Assignment")]
        public string ActivityName { get; set; }
        [Display(Name = "Description")]
        public string ActivityDescription { get; set; }
        [Display(Name = "Module")]
        public Entities.Module Module { get; set; }
        [Display(Name = "Start date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Display(Name = "Due date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [Display(Name = "Submitted")]
        public bool IsFinished { get; set; }
        [Display(Name = "Submission date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime? DocumentTimeStamp { get; set; }
        [Display(Name = "Late")]
        public bool IsLate { get; set; }
        public ICollection<Entities.Document> Documents { get; set; }
    }
}