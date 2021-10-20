using LMS.Core.Models.Entities;
using System.Threading.Tasks;

namespace LMS.Core.Repositories
{
    public interface IAccountRepository
    {
        Task AddAccount(Person person, string password, string role);
    }
}