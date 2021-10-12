using LMS.Api.Core.Entities;
using LMS.Api.ResourceParamaters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.Core.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAuthorsAsync(AuthorsResourceParameters authorResourceParameters);
        Task<Author> GetAuthorAsync(int? id, bool includeWorks);
        Task<Author> FindAsync(int? id);
        Task<bool> AnyAsync(int? id);
        Task AddAsync(Author Author);
        Task Update(Author Author);
        Task Remove(Author Author);
    }
}
