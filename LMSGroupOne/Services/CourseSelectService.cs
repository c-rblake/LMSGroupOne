using LMS.Data.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSGroupOne.Services
{
        public class CourseSelectService : ICourseSelectService
        {
            private readonly ApplicationDbContext db;

            public CourseSelectService(ApplicationDbContext db)
            {
                this.db = db;
            }

            public async Task<IEnumerable<SelectListItem>> GetCoursesAsync()
            {
                return await db.Courses.Where(c => c.StartDate >= DateTime.Now.AddDays(-10))
                    .Select(r => new SelectListItem
                    {
                        Text = r.Name.ToString(),
                        Value = r.Id.ToString()
                    }).ToListAsync();
            }
        }
 }