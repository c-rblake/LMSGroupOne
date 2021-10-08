using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Models.Entities
{
    public class Document
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public string DocumentUrl { get; set; }
        public DateTime TimeStamp { get; set; }
        [Required]
        public string PersonId { get; set; }

        public int? ModuleId { get; set; }
        public int? ActivityId { get; set; }
        public int? CourseId { get; set; }

        //Nav prop
        public Module Module { get; set; }
        public Activity Activity { get; set; }
        public Course Course { get; set; }
        
        public Person Person { get; set; }

    }
}
