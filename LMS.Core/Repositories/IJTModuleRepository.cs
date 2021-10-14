using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using LMS.Core.Dto;

namespace LMS.Core.Repositories
{
    public interface IJTModuleRepository
    {
        Task<ModuleDto> GetModule(int id, CancellationToken cancellationToken = default);
    }
}
