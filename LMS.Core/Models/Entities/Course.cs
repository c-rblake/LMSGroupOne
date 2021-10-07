using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Models.Entities
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        //[DisplayFormat(DataFormatString="{0:yyyy-MM-dd}", ApplyFormatInEditMode=true)]
        //in viewModel
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //in viewModel
        public DateTime? EndDate { get; set; }

        //NAV Properties
        public ICollection<Module> Modules { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<Person> Persons { get; set; }
    }
}
