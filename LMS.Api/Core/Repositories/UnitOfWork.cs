using LMS.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.Core.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LMSApiContext db;
        public IAuthorRepository AuthorRepository { get; }
        public IWorksRepository WorksRepository { get; }

        public UnitOfWork(LMSApiContext db)
        {
            this.db = db;
            this.AuthorRepository = new AuthorRepository(db);
            this.WorksRepository = new WorksRepository(db);
        }

        public Task CompleteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
