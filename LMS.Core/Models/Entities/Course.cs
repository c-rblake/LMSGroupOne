using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Models.Entities
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        //NAV Properties
        public ICollection<Module> Modules { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<Person> Persons { get; set; }
    }
}
