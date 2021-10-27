using LMS.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LMS.Core.Repositories
{
    public interface IJTPersonRepository
    {
        Task<Person> GetPerson(string id, CancellationToken cancellationToken = default);

        Task RemoveAsync(string id);
    }
}
