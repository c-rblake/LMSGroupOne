using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Models.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        //NAV Properties
        public ICollection<Module> Modules { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<Person> Persons { get; set; }
    }
}
