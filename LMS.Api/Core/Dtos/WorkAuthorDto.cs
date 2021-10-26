using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.Core.Dtos
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
