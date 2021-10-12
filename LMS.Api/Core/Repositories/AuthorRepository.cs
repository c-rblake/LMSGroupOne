using LMS.Api.Core.Entities;
using LMS.Api.Data;
using LMS.Api.ResourceParamaters;
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

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync(AuthorsResourceParameters authorResourceParameters)
        {
            var query = db.Authors.AsQueryable();

            if (authorResourceParameters.includeWorks)
            {
                query = query.Include(a => a.Works); //Todo Implement WorksDto 
            };

            return await query.ToListAsync();
        }

        public async Task<Author> GetAuthorAsync(int? id, bool includeworks=false)
        {
            var query = db.Authors.AsQueryable();

            if (includeworks)
            {
                query = query.Include(a => a.Works); //Todo Implement WorksDto 
            };

            return await query.FirstOrDefaultAsync(a=>a.Id == id);
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
