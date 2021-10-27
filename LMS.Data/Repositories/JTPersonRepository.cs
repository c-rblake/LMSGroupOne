using LMS.Core.Models.Entities;
using LMS.Core.Repositories;
using LMS.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LMS.Data.Repositories
{
    public class JTPersonRepository : IJTPersonRepository
    {
        private readonly ApplicationDbContext db;

        public JTPersonRepository(ApplicationDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }



        public async Task<Person> GetPerson(string id, CancellationToken cancellationToken = default)
        {
            var person = await db.Persons.FindAsync(id);

            return person;
        }

        public async Task RemoveAsync(string id)
        {
            var person = await db.Persons.FindAsync(id);

            db.Remove(person);
            
        }
    }
}
