using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.Core.Entities
{
    public class Work
    {
        public int Id { get; set; }
        public int GenreId { get; set; }
        public int TypeId { get; set; }

        public string Description { get; set; }

        public string Level { get; set; }

        public DateTime PublicationDate { get; set; }

        public ICollection<Author> Authors { get; set; }
        public Genre Genre { get; set; }

        public Type Type { get; set; }
    }
}
