using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Models.ViewModels.API.Author
{
    public class LibrarySearch
    {
        public string Name { get; set; } = "Tulip";
        public bool OrderOnAge { get; set; }

        public bool NameOrdered { get; set; }
    }
}
