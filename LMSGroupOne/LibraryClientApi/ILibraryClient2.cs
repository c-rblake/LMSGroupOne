using LMSGroupOne.LibraryClientApi;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LMSGroupOne.LibraryClientApi
{
    public interface ILibraryClient2
    {
        Task<IEnumerable<AuthorDto>> GetAllAuthors(CancellationToken token);
        Task<IEnumerable<WorkDto>> GetAllWorks(CancellationToken token);
        Task<AuthorDto> GetAuthor(CancellationToken token, int id);
    }
}