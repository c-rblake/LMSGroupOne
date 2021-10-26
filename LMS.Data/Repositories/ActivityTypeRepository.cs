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
    class ActivityTypeRepository : IActivityTypeRepository
    {
        private readonly ApplicationDbContext db;
        public ActivityTypeRepository(ApplicationDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public async Task<IEnumerable<ActivityType>> GetAsync()
        {
            return await db.ActivityTypes.ToListAsync();
        }
    }
}
