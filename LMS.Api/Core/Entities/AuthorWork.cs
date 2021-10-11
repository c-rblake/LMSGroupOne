using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.Core.Entities
{
    public class AuthorWork
    {
        public ICollection<Author> Authors { get; set; }
        public ICollection<Work> Works { get; set; }

    }
}
