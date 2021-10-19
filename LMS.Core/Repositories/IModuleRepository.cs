using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Core.Models.Entities;

namespace LMS.Core.Repositories
{
    public interface IModuleRepository
    {
        Task<IEnumerable<Module>> GetAsync();

        Task<Module> FindAsync(int? id);

        void AddModule(Module module);
        Task<Module> GetModule(int id);
        bool ModuleExists(string name);

        bool ModuleExistsById(int? id);
        Task<IEnumerable<Activity>> GetModulesByCourseId(int courseId);
    }
}
