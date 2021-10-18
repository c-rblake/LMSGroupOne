using System.Threading.Tasks;

namespace LMS.Core.Repositories
{
    public interface IUnitOfWork
    {
        ITeacherRepository TeacherRepository { get; }
        ICourseRepository CourseRepository { get; }
        IAccountRepository AccountRepository { get; }
        Task CompleteAsync();
    }
}