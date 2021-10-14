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
            

            //return new ActivityDto
            //{
            //    Id = activity.Id,
            //    Name = activity.Name,
            //    Description = activity.Description,
            //    StartDate = activity.StartDate,
            //    EndDate = activity.EndDate,
            //    //TypeId=activity.ActivityType.Id,
            //    //TypeName = activity.ActivityType.Name,
            //    //TypeDescription=activity.ActivityType.Description
            //};


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
    }
}
