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
    class CourseRepository:ICourseRepository
    {
        private readonly ApplicationDbContext db;

        public CourseRepository(ApplicationDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public void AddCourse(Course course)
        {
            db.AddAsync(course);
        }
    }
}
