using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Core.Repositories;
using LMS.Data.Data;

namespace LMS.Data.Repositories
{
    public class JTUnitOfWork : IJTUnitOfWork
    {
        private readonly ApplicationDbContext db;
        
        public IJTCourseRepository CourseRepository { get; }
        public IJTModuleRepository ModuleRepository { get; }
        public IJTActivityRepository ActivityRepository { get; }

        public IJTDocumentRepository DocumentRepository { get; }

        public IJTPersonRepository PersonRepository { get; }

        

        public JTUnitOfWork(ApplicationDbContext db)
        {
            this.db = db;
            
            CourseRepository = new JTCourseRepository(db);
            ModuleRepository = new JTModuleRepository(db);
            ActivityRepository = new JTActivityRepository(db);
            DocumentRepository = new JTDocumentRepository(db);
            PersonRepository = new JTPersonRepository(db);
        }

        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }

    }
}