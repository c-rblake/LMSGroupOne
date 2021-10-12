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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        //Nav properties
        public ICollection<AuthorWorkDto> WorkDtos { get; set; } //ToDo Circular WARNING Works <=> Authors
    }
}
