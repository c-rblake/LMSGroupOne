using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMSGroupOne.Services
{
    public interface IActivityTypeSelectService
    {
        Task<IEnumerable<SelectListItem>> GetActivityTypesAsync();

    }
}
