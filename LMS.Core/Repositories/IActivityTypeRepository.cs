using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Core.Models.Entities;

namespace LMS.Core.Repositories
{
    public interface IActivityTypeRepository
    {
        Task<IEnumerable<ActivityType>> GetAsync();
    }
}
