using System;
using System.Collections.Generic;

namespace LMS.Core.Models.Entities.API
{
    public class Work
    {
        public int Id { get; set; }
        public int GenreId { get; set; }
        public int TypeId { get; set; }

        public string Title { get; set; } //<string, Mappning + reverse>

        public string Description { get; set; }

        public string Level { get; set; }

        public DateTime PublicationDate { get; set; }

        public ICollection<Author> Authors { get; set; }
        public Genre Genre { get; set; }

        public Entities.API.Type Type { get; set; }
    }
}