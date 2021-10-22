using LMSGroupOne.LibraryClientApi;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LMSGroupOne.LibraryClientApi
{
    public interface ILibraryClient2
    {
        Task<IEnumerable<AuthorDto>> GetAllAuthors(CancellationToken token);
        Task<IEnumerable<WorkDto>> GetAllWorks(CancellationToken token);
        Task<AuthorDto> GetAuthor(CancellationToken token, string id);

        Task<Author> PostAuthor(Author author);
        //Task<T> PostAuthor2<T>(string str, object obj);

        //Task<(string, object)> PostAuthor2(string str, Object obj);
        //Task<(string, object)> PostAuthor2(string str, Author author);
        Task<string> PostAuthor2(Author author);
    }
}