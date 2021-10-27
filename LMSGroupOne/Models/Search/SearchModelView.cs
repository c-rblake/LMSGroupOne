using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSGroupOne.Models.Search
{
    public class SearchModelView
    {
        public string Name { get; set; }

        public IEnumerable<string> Courses { get; set; }

    }
}
