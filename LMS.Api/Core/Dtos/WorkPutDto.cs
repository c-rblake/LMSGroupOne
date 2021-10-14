using LMS.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.Core.Dtos
{
    public class WorkPutDto
    {
        //public int Id { get; set; }
        //public int GenreId { get; set; }
        //public int TypeId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Level { get; set; }

        public DateTime PublicationDate { get; set; }

        public ICollection<Author> Authors { get; set; }
        public Genre Genre { get; set; }

        public Entities.Type Type { get; set; }
    }
}
