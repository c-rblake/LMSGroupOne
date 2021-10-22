using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Models.ViewModels
{
    public class AuthorsViewmodel
    {
        public int Id { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }

        public string Name { get; set; }
        public int Age { get; set; }

        //Todo DateTimeOffset instead
        public DateTimeOffset DateOfBirth { get; set; }
        public DateTimeOffset? DateOfDeath { get; set; }

    }
}
