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
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
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


        public async Task<IEnumerable<TreeDataDto>> GetTreeData(CancellationToken cancellationToken=default)
        {
           
            //todo get users with teacher role?


            return null;   
        
        }

    }


      




    
}
