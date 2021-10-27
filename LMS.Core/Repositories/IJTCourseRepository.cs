using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using LMS.Core.Dto;

namespace LMS.Core.Repositories
{
    public interface IJTCourseRepository
    {
        
        Task<IEnumerable<TreeDataDto>> GetTreeData(CancellationToken cancellationToken = default);

        Task<IEnumerable<TreeDataDto>> GetTreeDataForStudent(string id, CancellationToken cancellationToken = default);
        Task<CourseDto> GetCourse(int id, CancellationToken cancellationToken = default);

        Task RemoveAsync(int id);
    }
}
