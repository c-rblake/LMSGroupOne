using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.Services
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationProperties{ get; set; }
        public bool Revert { get; set; }
    }
}
