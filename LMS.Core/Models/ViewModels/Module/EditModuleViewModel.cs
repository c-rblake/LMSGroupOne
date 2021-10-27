using LMSGroupOne.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Models.ViewModels.Module
{
    public class EditModuleViewModel: IModalViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }

        public int CourseId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [CheckModuleDates]
        public DateTime EndDate { get; set; }
        

        // 
        public bool Success { get; set; }  // creation status
        public string Message { get; set; }   // returnmessage
        public int ReturnId { get; set; }    // return id when created
        public Guid PersonId { get; set; }
    }
}
