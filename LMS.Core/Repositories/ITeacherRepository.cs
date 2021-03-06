using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LMS.Core.Dto;
using LMS.Core.Models.Entities;

namespace LMS.Core.Repositories
{
    public interface ITeacherRepository
    { 
        Task<IEnumerable<Person>> GetAsync();


        Task<IEnumerable<TreeDataDto>> GetTreeData(CancellationToken cancellationToken = default);
    }
}
