using LMS.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.ResourceParamaters
{
    public class WorksResourceParameters
    {
        //Todo Input Params for [FromBody] or [FromQuery]

        public string Title { get; set; }
        //public Author Author { get; set; }

        public string AuthorName { get; set; }

        //public string Description { get; set; }

        public string Level { get; set; }
        public string GenreName { get; set; }

        public string OrderBy { get; set; } = "Title";

        //public Core.Entities.Type Type { get; set; }
    }
}
