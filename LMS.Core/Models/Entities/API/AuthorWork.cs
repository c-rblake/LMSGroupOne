using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Models.Entities.API
{
    public class AuthorWork
    {
        public int WorkId { get; set; }
        public int AuthorId { get; set; }
        public ICollection<Author> Authors { get; set; }
        public ICollection<Work> Works { get; set; }

    }
}
