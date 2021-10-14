using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Core.Repositories;
using LMS.Core.Dto;
using LMS.Data.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace LMS.Data.Repositories
{
    public class JTModuleRepository:IJTModuleRepository
    {
        private readonly ApplicationDbContext db;

        public JTModuleRepository(ApplicationDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<ModuleDto> GetModule(int id, CancellationToken cancellationToken = default)
        {

            var module = await db.Modules.FindAsync(id);
            return new ModuleDto
            {
                Id=module.Id,
                Name=module.Name,
                Description=module.Description,
                StartDate=module.StartDate,
                EndDate=module.EndDate
            };
        }
    }
}
