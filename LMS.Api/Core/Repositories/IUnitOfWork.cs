using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Api.Core.Repositories
{
    public interface IUnitOfWork
    {
        IAuthorRepository AuthorRepository { get; }
        IWorksRepository WorksRepository { get; }

        Task CompleteAsync();
    }
}
