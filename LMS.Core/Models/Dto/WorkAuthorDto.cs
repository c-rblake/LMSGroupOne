using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Models.Dto
{
    public class WorkAuthorDto
    {
        /// <summary>
        /// A Class to Prevent Circular references. Work *-* Author
        /// </summary>
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        //Nav properties
        //public ICollection<Work> Works { get; set; }
    }

}
