using System;
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
        public Entities.Module Module { get; set; }
        [Display(Name = "Start date")]
        [DisplayFormat(DataFormatString = "yyyy-mm-dd")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Display(Name = "End date")]
        [DisplayFormat(DataFormatString = "yyyy-mm-dd")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [Display(Name = "Finished")]
        public bool IsFinished { get; set; }
        [Display(Name = "Delivery date")]
        [DisplayFormat(DataFormatString = "yyyy-mm-dd")]
        [DataType(DataType.Date)]
        public DateTime DocumentTimeStamp { get; set; }
        [Display(Name = "Late")]
        public bool IsLate { get; set; }
    }
}