using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Core.Models.Entities;
using LMS.Core.Repositories;
using LMS.Data.Data;
using Microsoft.AspNetCore.Identity;

namespace LMS.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<Person> userManager;
        public ITeacherRepository TeacherRepository { get;}
        public ICourseRepository CourseRepository { get; }
        public IAccountRepository AccountRepository { get; }

        public UnitOfWork(ApplicationDbContext db, UserManager<Person> userManager)
        {
            this.db = db;
            this.userManager = userManager;
            TeacherRepository = new TeacherRepository(db);
            CourseRepository = new CourseRepository(db);
            AccountRepository = new AccountRepository(userManager);
        }

        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}