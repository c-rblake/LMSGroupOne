using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Core.Models.Entities;
using LMS.Core.Repositories;
using LMS.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data.Repositories
{
    class ModuleRepository : IModuleRepository
    {
        private readonly ApplicationDbContext db;
        public ModuleRepository(ApplicationDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public void AddModule(Module module)
        {
            db.AddAsync(module);
        }

        public bool ModuleExist(string name)
        {
            return db.Modules.Any(c => c.Name == name);
        }

        public async Task<IEnumerable<Module>> GetAsync()
        {
            return await db.Modules.ToListAsync();
        }

        public async Task<Module> FindAsync(int? id)
        {
            return await db.Modules.FindAsync(id);
        }

        public bool ModuleExistsById(int? id)
        {

            return db.Modules.Any(e => e.Id == id);
        }

        public async Task<Module> GetModule(int id)
        {
            return await db.Modules.FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
