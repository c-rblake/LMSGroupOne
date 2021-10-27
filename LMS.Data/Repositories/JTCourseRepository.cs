using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using LMS.Core.Repositories;
using LMS.Core.Dto;
using LMS.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data.Repositories
{

    

    

    class JTCourseRepository:IJTCourseRepository
    {

        private readonly ApplicationDbContext db;

        public JTCourseRepository(ApplicationDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }


        public async Task RemoveAsync(int id)
        {
            var course = await db.Courses.FindAsync(id);

            var modules = db.Modules.Where(m => m.CourseId == course.Id);
            foreach (var mod in modules)
            {
                var activities = db.Activities.Where(a => a.ModuleId == mod.Id);
                foreach (var act in activities)
                {
                    var adoc = db.Documents.Where(z => z.ActivityId == act.Id);
                    db.Documents.RemoveRange(adoc);
                    db.Activities.Remove(act);
                }
                var mdoc = db.Documents.Where(z => z.ModuleId == mod.Id);
                db.Documents.RemoveRange(mdoc);

            }
            var cdoc = db.Documents.Where(z => z.CourseId == course.Id);
            db.Documents.RemoveRange(cdoc);


            // remove students
            var peps=db.Persons.Where(p => p.CourseId == course.Id);
            db.Persons.RemoveRange(peps);



            db.Courses.Remove(course);





            //var activities = db.Activities.Where(m => m.ModuleId == id);

            //foreach (var act in activities)
            //{
            //    var adoc = db.Documents.Where(z => z.ActivityId == act.Id);
            //    db.Documents.RemoveRange(adoc);
            //    db.Activities.Remove(act);
            //}


            //var doc = db.Documents.Where(z => z.ModuleId == id);
            //db.Documents.RemoveRange(doc)



        }







        public async Task<IEnumerable<TreeDataDto>> GetTreeData(CancellationToken cancellationToken = default)
        {
            // todo refactor with below
            var model = db.Courses.Select(c => new TreeDataDto
            {
                Id = c.Id.ToString(),
                Name = c.Name,
                Nodes = c.Modules.Select(m => new TreeDataDto
                {
                    Id = m.Id.ToString(),
                    Name = m.Name,
                    Nodes = m.Activities.Select(a => new TreeDataDto
                    {
                        Id = a.Id.ToString(),
                        Name = a.Name,
                        Nodes = null,
                        Documents = a.Documents.Select(ad => new TreeDataDto
                        {
                            Id = ad.Id.ToString(),
                            Name = ad.Name,
                            Nodes = null,
                            Documents = null,
                            Persons = null,
                        }),
                        Persons = null,
                    }),
                    Documents = null,
                    Persons = null
                }),
                Documents = c.Documents.Select(d => new TreeDataDto
                {
                    Id = d.Id.ToString(),
                    Name = d.Name,
                    Nodes = null,
                    Documents = null,
                    Persons = null
                }),
                Persons = c.Persons.Select(p => new TreeDataDto
                {
                    Id = p.Id.ToString(),
                    Name = $"{p.FirstName} {p.LastName}",
                    Nodes = null,
                    Documents = null,
                    Persons = null
                })

            });



            return await model.ToListAsync(cancellationToken);
        }



        public async Task<IEnumerable<TreeDataDto>> GetTreeDataForStudent(string id, CancellationToken cancellationToken = default)
        {
            // todo refactor with above
            var courseId = db.Persons.Where(p => p.Id==id && p!=null).Select(s=>s.CourseId).FirstOrDefault();
            var model = db.Courses.Where(c=>c.Id==courseId).Select(c => new TreeDataDto
            {
                Id = c.Id.ToString(),
                Name = c.Name,
                Nodes = c.Modules.Select(m => new TreeDataDto
                {
                    Id = m.Id.ToString(),
                    Name = m.Name,
                    Nodes = m.Activities.Select(a => new TreeDataDto
                    {
                        Id = a.Id.ToString(),
                        Name = a.Name,
                        Nodes = null,
                        Documents = a.Documents.Select(ad => new TreeDataDto
                        {
                            Id = ad.Id.ToString(),
                            Name = ad.Name,
                            Nodes = null,
                            Documents = null,
                            Persons = null,
                        }),
                        Persons = null,
                    }),
                    Documents = null,
                    Persons = null
                }),
                Documents = c.Documents.Select(d => new TreeDataDto
                {
                    Id = d.Id.ToString(),
                    Name = d.Name,
                    Nodes = null,
                    Documents = null,
                    Persons = null
                }),
                Persons = c.Persons.Select(p => new TreeDataDto
                {
                    Id = p.Id.ToString(),
                    Name = $"{p.FirstName} {p.LastName}",
                    Nodes = null,
                    Documents = null,
                    Persons = null
                })

            });



            return await model.ToListAsync(cancellationToken);
        }




        public async Task<CourseDto> GetCourse(int id, CancellationToken cancellationToken = default)
        {
            var course = await db.Courses.FindAsync(id);

            if (course == null)
            {
                return null;
            }

            return new CourseDto
            {
                Id = course.Id,
                Name = course.Name,
                Description=course.Description,
                StartDate=course.StartDate,
                EndDate=(DateTime)course.EndDate
            };

        }
    }
}

