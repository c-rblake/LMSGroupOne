using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Core.Models.Entities;
using LMS.Core.Repositories;
using LMS.Data.Data;

namespace LMS.Data.Repositories
{
    class ActivityRepository : IActivityRepository
    {
        private readonly ApplicationDbContext db;
        public ActivityRepository(ApplicationDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public void AddActivity(Activity activity)
        {
            db.AddAsync(activity);
        }

        public bool ActivityExists(string name)
        {
            return db.Activities.Any(c => c.Name == name);
        }

        public async Task<Activity> FindAsync(int? id)
        {
             return await db.Activities.FindAsync(id);
        }

        public bool ActivityExistsById(int? id)
        {

            return db.Activities.Any(e => e.Id == id);
        }
    }
}
