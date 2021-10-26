using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMSGroupOne.Services
{
    public interface IRoleSelectService
    {
        Task<IEnumerable<SelectListItem>> GetRolesAsync();
    }
}