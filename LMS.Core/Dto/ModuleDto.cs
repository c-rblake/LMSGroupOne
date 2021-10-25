using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Dto
{
    public class ModuleDto
    {
        public int Id { get; set; }        
        public string Name { get; set; }
        public string Description { get; set; }
        public int CourseId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
