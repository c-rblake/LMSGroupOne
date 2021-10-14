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
            var works = await db.Works
                .Include(w => w.Authors)
                .Include(w => w.Genre)
                .Include(w => w.Type)
                .ToListAsync();

            return works;
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
