using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Repositories
{
    public interface IUnitOfWorks
    {
        ITeacherRepository TeacherRepository { get; }
        Task CompleteAsync();
    }
}
