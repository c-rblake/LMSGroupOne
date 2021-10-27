using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using LMS.Core.Dto;
using LMS.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using LMS.Data.Data;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace LMS.Data.Repositories
{
    public class JTActivityRepository:IJTActivityRepository
    {
        private readonly ApplicationDbContext db;

        public JTActivityRepository(ApplicationDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<ActivityDto> GetActivity(int id, CancellationToken cancellationToken = default)
        {
            var activity = await db.Activities.FindAsync(id);

            

            var ak=db.Activities.Where(i => i.Id == id).Select(a => new ActivityDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                TypeId=a.ActivityType.Id,
                TypeName=a.ActivityType.Name,
                TypeDescription=a.ActivityType.Description
            });

            return await ak.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var activity = await db.Activities.FindAsync(id);

            var doc = db.Documents.Where(z => z.ActivityId == id);
            db.Documents.RemoveRange(doc);


            db.Remove(activity);

        }
    }
}
