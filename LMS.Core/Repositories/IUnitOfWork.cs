using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Repositories
{
    public interface IUnitOfWork
    {
        ITeacherRepository TeacherRepository { get; }
        ICourseRepository CourseRepository { get; }
        IActivityRepository ActivityRepository { get; }
        IActivityTypeRepository ActivityTypeRepository { get; }
        IModuleRepository ModuleRepository { get; }
        Task CompleteAsync();
    }
}
