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
    class TeacherRepository : ITeacherRepository
    {
        private readonly ApplicationDbContext db;

        public TeacherRepository(ApplicationDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public async Task<IEnumerable<Person>> GetAsync()
        {
            return await db.Persons.ToListAsync();
        }
    }
}
