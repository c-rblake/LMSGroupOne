using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Models.ViewModels.API.Work
{
    public class WorksViewModel
    {
        public int Id { get; set; }

        public int GenreId { get; set; }
        public int TypeId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Level { get; set; }

        public DateTime PublicationDate { get; set; }
    }
}
