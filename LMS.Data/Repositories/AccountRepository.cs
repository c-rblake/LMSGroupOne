using LMS.Core.Models.Entities;
using LMS.Core.Repositories;
using LMS.Data.Data;
using System;

namespace LMS.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext db;

        public AccountRepository(ApplicationDbContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public void AddAccount(Person person)
        {
            db.AddAsync(person);
        }
    }
}