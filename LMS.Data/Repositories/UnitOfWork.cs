using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Core.Repositories;
using LMS.Data.Data;

namespace LMS.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext db;
        public ITeacherRepository TeacherRepository { get;}
        public ICourseRepository CourseRepository { get; }
        public IAccountRepository AccountRepository { get; }

        public UnitOfWork(ApplicationDbContext db)
        {
            this.db = db;
            TeacherRepository = new TeacherRepository(db);
            CourseRepository = new CourseRepository(db);
            AccountRepository = new AccountRepository(db);
        }

        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}