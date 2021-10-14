using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Dto
{
    public class TreeDataDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        
        public IEnumerable<TreeDataDto> Nodes { get; set; }
        public IEnumerable<TreeDataDto> Documents { get; set; }

        public IEnumerable<TreeDataDto> Persons { get; set; }



    }
}
