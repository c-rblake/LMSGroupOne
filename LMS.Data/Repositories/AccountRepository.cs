using LMS.Core.Models.Entities;
using LMS.Core.Repositories;
using Microsoft.AspNetCore.Identity;

namespace LMS.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<Person> userManager;

        public AccountRepository(UserManager<Person> userManager)
        {
            //this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            //if (userManager is null) throw new NullReferenceException(nameof(UserManager<Person>));
        }

        public void AddAccount(Person person)
        {
            //var result1 = userManager.CreateAsync(person, Password);
            //if (!result1.Succeeded) throw new Exception(string.Join("\n", result1.Errors));
            //var result2 = userManager.AddToRoleAsync(person, Role);
            //if (!result2.Succeeded) throw new Exception(string.Join("\n", result2.Errors));
        }
    }
}