using LMS.Core.Models.Entities.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Models.ViewModels.API.Author
{
    public class AuthorsViewModel
    {
        public int Id { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }

        public string Name { get; set; }
        public int Age { get; set; }

        //Todo DateTimeOffset instead
        public DateTimeOffset DateOfBirth { get; set; }
        public DateTimeOffset? DateOfDeath { get; set; }

        public List<Entities.API.Work> Works { get; set; } = new List<Entities.API.Work>();
    }
}
