﻿using System;
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
        public void AddCourse(Course course)
        {
            db.AddAsync(course);
        }




        // get some data for the tree view

        public async Task<IEnumerable<TreeDataDto>> GetTreeData(CancellationToken cancellationToken=default)
        {
            // todo option one course
            var model = db.Courses.Select(c => new TreeDataDto
            {
                Id = c.Id.ToString(),
                Name = c.Name,
                Nodes = c.Modules.Select(m => new TreeDataDto
                {
                    Id = m.Id.ToString(),
                    Name = m.Name,
                    Nodes = m.Activities.Select(a=> new TreeDataDto 
                    { 
                        Id=a.Id.ToString(),
                        Name=a.Name,
                        Nodes=null,
                        Documents=a.Documents.Select(ad=>new TreeDataDto 
                        { 
                            Id=ad.Id.ToString(),
                            Name=ad.Name,
                            Nodes=null,
                            Documents=null,
                            Persons=null,
                        }),
                        Persons=null,
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
                    Id=p.Id.ToString(),
                    Name=$"{p.FirstName} {p.LastName}",
                    Nodes=null,
                    Documents=null,
                    Persons=null
                })

            });

            

            return await model.ToListAsync(cancellationToken);
        }

        public async Task<CourseDto> GetCourse(string id, CancellationToken cancellationToken = default)
        {
            var course=await db.Courses.FindAsync(id, cancellationToken);
            return new CourseDto
            {
                Id = course.Id.ToString(),
                Name=course.Name
            };
                   
        }
    }
}
