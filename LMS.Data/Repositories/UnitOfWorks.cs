using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Core.Repositories;
using LMS.Data.Data;

namespace LMS.Data.Repositories
{
    public class UnitOfWorks : IUnitOfWorks
    {
        private readonly ApplicationDbContext db;
        public ITeacherRepository TeacherRepository { get;}

        public UnitOfWorks(ApplicationDbContext db)
        {
            this.db = db;
            TeacherRepository = new TeacherRepository(db);
        }

        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
