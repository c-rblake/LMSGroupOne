using LMS.Data.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSGroupOne.Services
{
    public class RoleSelectService : IRoleSelectService
    {
        private readonly ApplicationDbContext db;

        public RoleSelectService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<SelectListItem>> GetRolesAsync()
        {
            return await db.Roles
                .Select(r => new SelectListItem
                {
                    Text = r.Name.ToString(),
                    Value = r.Id.ToString()
                }).ToListAsync();
        }
    }
}