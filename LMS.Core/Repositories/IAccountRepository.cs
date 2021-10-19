using LMS.Core.Models.Entities;

namespace LMS.Core.Repositories
{
    public interface IAccountRepository
    {
        void AddAccount(Person person, string password, string role);
    }
}