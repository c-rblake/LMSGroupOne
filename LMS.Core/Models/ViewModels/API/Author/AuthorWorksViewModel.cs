//using LMS.Core.Models.ViewModels.API.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Core.Models.Entities.API;

namespace LMS.Core.Models.ViewModels.API.Author
{
    public class AuthorWorksViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        //Nav properties
        public ICollection<Entities.API.Work> Works { get; set; }
    }
}
