using LMS.Core.Models.Entities;
using LMS.Core.Repositories;
using LMS.Data.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<Person> userManager;
        private readonly ApplicationDbContext db;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountRepository(UserManager<Person> userManager, ApplicationDbContext db, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            if (userManager is null) throw new NullReferenceException(nameof(UserManager<Person>));

            this.db = db ?? throw new ArgumentNullException(nameof(db));

            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            if (roleManager is null) throw new NullReferenceException(nameof(RoleManager<IdentityRole>));

        }

        public async Task AddAccount(Person person, string password, string role)
        {
            var result1 = await userManager.CreateAsync(person, password);
            if (!result1.Succeeded) throw new Exception(string.Join("\n", result1.Errors));
            var result2 = await userManager.AddToRoleAsync(person, role);
            if (!result2.Succeeded) throw new Exception(string.Join("\n", result2.Errors));
        }

        public async Task UpdateAccount(Person person, string password, string role)
        {
            var result1 = await userManager.UpdateAsync(person);
            if (!result1.Succeeded) throw new Exception(string.Join("\n", result1.Errors));
            var result2 = await userManager.AddToRoleAsync(person, role);
            if (!result2.Succeeded) throw new Exception(string.Join("\n", result2.Errors));
        }

        public async Task<Person> FindByIdAsync(string id)
        {
            return await userManager.FindByIdAsync(id);
        }

        public async Task<IdentityRole> RoleFindAsync(string id)
        {
            var roleId = db.UserRoles
                .Where(r => r.UserId == id)
                .Select(i => i.RoleId)
                .ToString();  //FindAsync(id);

            return await roleManager.FindByIdAsync(roleId);
        }

        public async Task RoleUpdateAsync(Person account, string oldRoleName, string newRoleName)
        {
            var result1 = await userManager.RemoveFromRoleAsync(account, oldRoleName);
            if (!result1.Succeeded) throw new Exception(string.Join("\n", result1.Errors));
            var result2 = await userManager.AddToRoleAsync(account, newRoleName);
            if (!result2.Succeeded) throw new Exception(string.Join("\n", result2.Errors));
        }

        public async Task<bool> EmailExists(string email)
        {
            return await db.Users.AnyAsync(e => e.Email == email);
        }

        public async Task<IdentityUser> GetUserAsync(string id)
        {
            return await db.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        }
    }
}