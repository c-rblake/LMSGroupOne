using LMS.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.Core.Dtos
{
    public class AuthorDto
    {
        public int Id { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }

        public string Name { get; set; }
        public int Age { get; set; }

        //Nav properties
        public ICollection<AuthorWorkDto> Works { get; set; } // Circular WARNING Works <=> Authors
                                                                 // Todo Also change the Parameter Name to something presentable such as Works
    }
}
