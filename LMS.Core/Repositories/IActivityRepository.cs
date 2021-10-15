using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Core.Models.Entities;

namespace LMS.Core.Repositories
{
    public interface IActivityRepository
    {
        void AddActivity(Activity activity);
        bool ActivityExists(string name);
        bool ActivityExistsById(int? id);
        Task<Activity> FindAsync(int? id);
    }
}
