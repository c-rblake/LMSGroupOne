using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Dto
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DocumentUrl { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PersonId { get; set; }        
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }

    }
}
