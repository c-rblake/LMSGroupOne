using LMS.Api.Core.Entities;
using LMS.Api.Data;
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

        public Task AddAsync(Work work)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<Work> FindAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Work>> GetAllWorksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Work> GetWorkAsync(int? id)
        {
            throw new NotImplementedException();
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
