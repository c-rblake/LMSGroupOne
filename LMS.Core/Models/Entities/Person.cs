using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Models.Entities
{
    public class Person : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CourseId { get; set; }

        //NavProperties
        public ICollection<Document> Documents { get; set; }
        public Course Course { get; set; }

    }
}
