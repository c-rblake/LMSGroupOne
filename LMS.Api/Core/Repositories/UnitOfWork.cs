using LMS.Api.Data;
using LMS.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.Core.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LMSApiContext db;
        private readonly IPropertyMappingService propertyMappingService;

        public IAuthorRepository AuthorRepository { get; }
        public IWorksRepository WorksRepository { get; }

        public UnitOfWork(LMSApiContext db, IPropertyMappingService propertyMappingService)
        {
            this.db = db;
            this.propertyMappingService = propertyMappingService;
            this.AuthorRepository = new AuthorRepository(db, propertyMappingService);
            this.WorksRepository = new WorksRepository(db);
        }

        public async Task<bool> CompleteAsync()
        {
            return (await db.SaveChangesAsync()) >= 0;
        }
    }
}
