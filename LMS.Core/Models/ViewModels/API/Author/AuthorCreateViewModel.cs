using LMS.Core.Models.Entities.API;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Models.ViewModels.API
{
    public class AuthorCreateViewModel:IModalViewModel
    {
        //public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public DateTimeOffset? DateOfDeath { get; set; }

        public bool Success { get; set; }
        public string Message { get; set; }
        public int ReturnId { get; set; }
        public string PersonReturnId { get; set; }

        //public ICollection<Work> Works { get; set; }
    }
}
