using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Repositories
{
    public interface IJTUnitOfWork
    {        
        IJTCourseRepository CourseRepository { get; }
        IJTModuleRepository ModuleRepository { get; }

        IJTActivityRepository ActivityRepository { get; }

    }
}
