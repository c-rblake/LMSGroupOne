using LMS.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.ResourceParamaters
{
    public class WorksResourceParameters
    {
        const int maxPageSize = 15;
        //Todo Input Params for [FromBody] or [FromQuery]

        public string Title { get; set; }
        //public Author Author { get; set; }

        public string AuthorName { get; set; }

        //public string Description { get; set; }

        public string Level { get; set; }
        public string GenreName { get; set; }

        public string OrderBy { get; set; } = "Title";

        public int PageNumber { get; set; } = 1;

        private int _pageSize { get; set; } = 10;
        public int PageSize 
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        } 

        //public Core.Entities.Type Type { get; set; }
    }
}
