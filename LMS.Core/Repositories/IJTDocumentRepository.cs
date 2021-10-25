using LMS.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LMS.Core.Repositories
{
    public interface IJTDocumentRepository
    {

        Task<DocumentDto> GetDocument(int id, CancellationToken cancellationToken = default);

        Task RemoveAsync(int id);

    }
}
