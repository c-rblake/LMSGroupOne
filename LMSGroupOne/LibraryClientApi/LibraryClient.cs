using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LMSGroupOne.LibraryClientApi
{
    public class LibraryClient
    {
        public HttpClient HttpClient { get; }
        public LibraryClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

    }
}
