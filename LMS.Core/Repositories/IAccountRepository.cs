using LMS.Core.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace LMS.Core.Repositories
{
    public interface IAccountRepository
    {
        Task AddAccount(Person person, string password, string role);

        Task<Person> FindByIdAsync (string id);

        Task<IdentityRole> RoleFindAsync(string id);

        Task RoleUpdateAsync(Person account, string oldRoleName, string newRoleName);

        Task<bool> EmailExists(string email);

        Task<IdentityUser> GetUserAsync(string id);
    }
}