using LMSGroupOne.LibraryClientApi;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace LMSGroupOne.Controllers
{
    public class LibraryController:Controller
    {
        private readonly LibraryClient libraryClient;
        private readonly ILibraryClient2 libraryClient2;

        public LibraryController(LibraryClient libraryClient, ILibraryClient2 libraryClient2) //Todo dependency injection goes here.
        {
            this.libraryClient = libraryClient;
            this.libraryClient2 = libraryClient2;
        }

        public async Task<IActionResult> IndexAsync() //Testmethods
        {
            var cancellation = new CancellationTokenSource();
            var r1 = await libraryClient2.GetAllAuthors(cancellation.Token);
            var r2 = await libraryClient2.GetAllWorks(cancellation.Token);   //todo null                            
            //var r3 = await libraryClient2.GetAuthor(cancellation.Token, 5); Todo
            var res = await GetWithStreamAndFactory();


            return View();
        }


        //Moved to BaseClientClass
        private async Task<IEnumerable<AuthorDto>> GetWithStreamAndFactory() // Requires a lot of information. AutoGenerate DTO. 
        {
            IEnumerable<AuthorDto> authorDtos; //Field to hold authorDtos

            //REQUEST
            var request = new HttpRequestMessage(HttpMethod.Get, "api/authors");
            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //RESPONS
            var response = await libraryClient.HttpClient.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead);
            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                response.EnsureSuccessStatusCode();

                using (var streamReader = new StreamReader(stream)) //read stream
                {
                    using (var jsonReader = new JsonTextReader(streamReader)) // NewtonSoft to read json
                    {
                        //Transform object to the right format.
                        var serializer = new Newtonsoft.Json.JsonSerializer(); //Not built in JsonSerializer
                        authorDtos = serializer.Deserialize<IEnumerable<AuthorDto>>(jsonReader);
                    }
                }
            }
            return authorDtos;
        }


    }
    public class LibraryClient2 : BaseClient, ILibraryClient2
    {
        private readonly HttpClient httpClient;

        public LibraryClient2(HttpClient httpClient) : base(httpClient, new Uri("https://localhost:44308/")) //send to base constructor
        {
            //this.httpClient = httpClient;
            //HttpClient.BaseAddress = new Uri("https://localhost:44308/");
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<AuthorDto>> GetAllAuthors(CancellationToken token)
        {
            return await base.GetAsync<IEnumerable<AuthorDto>>(token, "api/authors"); //

        }
        public async Task<AuthorDto> GetAuthor(CancellationToken token, int id)
        {
            // return await base.GetAsync<CodeEventDto>(token, $"api/events/{name}");
            return await base.GetAsync<AuthorDto>(token, $"api/events/{id}");
        }
        public async Task<IEnumerable<WorkDto>> GetAllWorks(CancellationToken token)
        {
            return await base.GetAsync<IEnumerable<WorkDto>>(token, "api/authors");
        }


    }

    public class BaseClient
    {
        protected HttpClient HttpClient { get; }

        public BaseClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public BaseClient(HttpClient httpClient, Uri uri) : this(httpClient) //overloaded and Chained constructor
        {
            HttpClient.BaseAddress = uri;
        }

        public async Task<T> GetAsync<T>(CancellationToken token, string path, HttpMethod method = null, string contentType = "application/json") // Requires a lot of information. AutoGenerate DTO.
        {
            T dtos; //Field to hold authorDtos
            if (method is null)
            {
                method = HttpMethod.Get; //More for fun
            }
                
            //REQUEST
            var request = new HttpRequestMessage(method, path); //ToDo HttpMethod.Get
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

            //RESPONS
            var response = await HttpClient.SendAsync(request,  // libraryClient.HttpClient replaced for Generic <T> method.
                HttpCompletionOption.ResponseHeadersRead,
                token);
            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                response.EnsureSuccessStatusCode();

                using (var streamReader = new StreamReader(stream)) //read stream
                {
                    using (var jsonReader = new JsonTextReader(streamReader)) // NewtonSoft to read json
                    {
                        //Transform object to the right format.
                        var serializer = new Newtonsoft.Json.JsonSerializer(); //Not built in JsonSerializer
                        dtos = serializer.Deserialize<T>(jsonReader); //Return here.
                    }
                }
            }
            return dtos;
        }


    }
}
