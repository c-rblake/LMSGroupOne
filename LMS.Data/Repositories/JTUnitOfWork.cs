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

        public JTUnitOfWork(ApplicationDbContext db)
        {
            this.db = db;
            
            CourseRepository = new JTCourseRepository(db);
            ModuleRepository = new JTModuleRepository(db);
            ActivityRepository = new JTActivityRepository(db);
        }

       
    }
}