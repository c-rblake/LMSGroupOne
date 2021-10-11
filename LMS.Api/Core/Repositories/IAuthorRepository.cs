using LMS.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.Core.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAuthorsAsync();
        Task<Author> GetAuthorAsync(int? id);
        Task<Author> FindAsync(int? id);
        Task<bool> AnyAsync(int? id);
        Task AddAsync(Author Author);
        Task Update(Author Author);
        Task Remove(Author Author);
    }
}
