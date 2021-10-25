using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LMS.Core.Dto;
using LMS.Core.Models.Entities;
using LMS.Core.Repositories;
using LMS.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data.Repositories
{
    class CourseRepository:ICourseRepository
    {
        private readonly ApplicationDbContext db;

        public CourseRepository(ApplicationDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<IEnumerable<Course>> GetAsync()
        {
            return await db.Courses.ToListAsync();
        }

        public void AddCourse(Course course)
        {
            db.AddAsync(course);
        }

        public bool CourseExist(string name)
        {
            return db.Courses.Any(c => c.Name == name);
        }

        public bool CourseExistById(int id)
        {
            return db.Courses.Any(e => e.Id == id);
          
        }

        public async Task<Course> FindAsync(int? id)
        {
            return await db.Courses.FindAsync(id);
        }

        public async Task<Course> GetCourse(int id)
        {
            return await db.Courses.FirstOrDefaultAsync(m => m.Id == id);
        }

        public void Update(Course course)
        {
            db.Update(course);
        }

        public int GetCourseId(string name)
        {
            return db.Courses.FirstOrDefault(c => c.Name == name).Id;
        }
    }
}
