using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMSGroupOne.Validations;

namespace LMS.Core.Models.ViewModels.Course
{
    public class CourseEditViewModel: IModalViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        [CheckDates]
        public DateTime EndDate { get; set; }


        public bool Success { get; set; }  // creation status
        public string Message { get; set; }   // returnmessage
        public int ReturnId { get; set; }    // return id when created
        public Guid PersonId { get; set; }

    }
}
