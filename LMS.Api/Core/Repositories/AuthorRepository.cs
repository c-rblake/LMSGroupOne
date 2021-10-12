using LMS.Api.Core.Entities;
using LMS.Api.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.Core.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LMSApiContext db;

        public AuthorRepository(LMSApiContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public Task AddAsync(Author Author)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<Author> FindAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Author> GetAuthorAsync(int? id)
        {
            var query = db.Authors.FirstOrDefaultAsync(a => a.Id == id);

            return await query;
        }

        public Task Remove(Author Author)
        {
            throw new NotImplementedException();
        }

        public Task Update(Author Author)
        {
            throw new NotImplementedException();
        }
    }
}
