using LMS.Core.Models.Entities;
using LMS.Core.Repositories;
using LMS.Data.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace LMS.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public UserManager<Person> userManager;

        public AccountRepository(UserManager<Person> userManager)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            if (userManager is null) throw new NullReferenceException(nameof(UserManager<Person>));
        }

        public async Task AddAccount(Person person, string password, string role)
        {
            var result1 = await userManager.CreateAsync(person, password);
            if (!result1.Succeeded) throw new Exception(string.Join("\n", result1.Errors));
            var result2 = await userManager.AddToRoleAsync(person, role);
            if (!result2.Succeeded) throw new Exception(string.Join("\n", result2.Errors));
        }
    }
}