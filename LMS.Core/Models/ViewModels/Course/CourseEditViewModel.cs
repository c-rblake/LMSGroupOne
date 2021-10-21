using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMSGroupOne.Validations;

namespace LMS.Core.Models.ViewModels.Course
{
    public class CourseEditViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        [CheckDates]
        public DateTime EndDate { get; set; }

    }
}
