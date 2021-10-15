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
    public class WorksRepository : IWorksRepository
    {
        private readonly LMSApiContext db;

        public WorksRepository(LMSApiContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task AddAsync(Work work)
        {
            await db.Works.AddAsync(work);
        }

        public Task<bool> AnyAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<Work> FindAsync(int? id)
        {
            return await db.Works.FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<IEnumerable<Work>> GetAllWorksAsync(WorksResourceParameters workResourceParameters)
        {
            //var works = await db.Works
            //    .Include(w => w.Authors)
            //    .Include(w => w.Genre)
            //    .Include(w => w.Type)
            //    .ToListAsync();

            var query = db.Works
                .Include(w => w.Authors)
                .Include(w => w.Genre)
                .Include(w => w.Type)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(workResourceParameters.Title))
            {
                query = query.Where(q => q.Title.Contains(workResourceParameters.Title)); //== workResourceParameters.Title);
            }
            if (!string.IsNullOrWhiteSpace(workResourceParameters.AuthorName))
            {
                var authors = db.Authors.Where(a => a.FirstName.Contains(workResourceParameters.AuthorName) || a.LastName.Contains(workResourceParameters.AuthorName));

                //query = query.Include(q => q.Authors.Where(a => author.Contains(a))); //Empty Author Lists
                query = query.Where(q => q.Authors.Any(a => authors.Contains(a)));
                //var result = lista.Where(a => listb.Any(b => b.Contains(a)));

            }
            if (!string.IsNullOrWhiteSpace(workResourceParameters.GenreName))
            {
                query = query.Where(q => q.Genre.Name == workResourceParameters.GenreName);

            }
            if (!string.IsNullOrWhiteSpace(workResourceParameters.OrderBy)) // On title
            {
                if (workResourceParameters.OrderBy.ToLowerInvariant() == "title")
                {
                    query = query.OrderBy(q => q.Title);
                }
            }

                return await query.ToListAsync();
        }

        public async Task<Work> GetWorkAsync(int id)
        {
            var work = await db.Works
                .Include(w => w.Genre)
                .Include(w => w.Authors) //Todo. SelfReference loop.
                .Include(w => w.Type)
                .FirstOrDefaultAsync(w => w.Id == id);

            return work;
        }

        public Task Remove(Work work)
        {
            throw new NotImplementedException();
        }

        public Task Update(Work work)
        {
            throw new NotImplementedException();
        }
    }
}
