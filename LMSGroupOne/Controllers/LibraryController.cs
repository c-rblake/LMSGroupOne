using LMSGroupOne.LibraryClientApi;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace LMSGroupOne.Controllers
{
    public class LibraryController : Controller
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
            var r3 = await libraryClient2.GetAuthor(cancellation.Token, "5"); //Todo
            var res = await GetWithStreamAndFactory();

            const string AuthorDTOJSON = "{'Id':'5', 'Name':'Al Bundy', 'Age': '43'}";
            AuthorDto author = JsonConvert.DeserializeObject<AuthorDto>(AuthorDTOJSON);
            dynamic authordynamic = JsonConvert.DeserializeObject(AuthorDTOJSON);

            var text = Activator.CreateInstance(System.Type.GetTypeFromProgID("Excel.Application", throwOnError: true));


            return View();
        }

        public async Task<IActionResult> Authors()
        {
            var cancellation = new CancellationTokenSource();
            IEnumerable<AuthorDto> authorDtos;

            authorDtos = await libraryClient2.GetAllAuthors(cancellation.Token);

            return View(authorDtos);
            //return View();

        }
        public async Task<IActionResult> Create(CreateAuthorViewModel createAuthorViewModel)
        {
            if (ModelState.IsValid)
                Console.WriteLine("");

            var author = new Author
            {
                FirstName = createAuthorViewModel.FirstName,
                LastName = createAuthorViewModel.LastName,
                DateOfBirth = createAuthorViewModel.DateOfBirth,
                DateOfDeath = createAuthorViewModel.DateOfDeath,
            };
            author.Works.Add(new Work
            {
                PublicationDate = createAuthorViewModel.PublicationDate,
                GenreId = createAuthorViewModel.GenreId,
                TypeId = createAuthorViewModel.TypeId,
                Description = createAuthorViewModel.Description,
                Title = createAuthorViewModel.Title,
                Level = createAuthorViewModel.Level
            });
            //Post

            var result = await libraryClient2.PostAuthor(author);
            //var (response, obje) = await libraryClient2.PostAuthor2("",author);
            var response = await libraryClient2.PostAuthor2(author);



            return View();
        }


        //Moved to BaseClientClass
        private async Task<IEnumerable<AuthorDto>> GetWithStreamAndFactory() // Requires a lot of information. AutoGenerate DTO. 
        {
            IEnumerable<AuthorDto> authorDtos; //Field to hold authorDtos
            IEnumerable<dynamic> jsonData;

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

                        //dynamic config = JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());
                        //jsonData = serializer.Deserialize<IEnumerable<dynamic>>(jsonReader);
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
            //return await base.GetAsync<IEnumerable<AuthorDto>>(token, "api/authors");
            return await base.GetAsync<IEnumerable<AuthorDto>>(token, "api/authors/?includeWorks=true");

        }
        public async Task<AuthorDto> GetAuthor(CancellationToken token, string id)
        {
            // return await base.GetAsync<CodeEventDto>(token, $"api/events/{name}");
            return await base.GetAsync<AuthorDto>(token, $"api/authors/{id}");
        }
        public async Task<IEnumerable<WorkDto>> GetAllWorks(CancellationToken token)
        {
            return await base.GetAsync<IEnumerable<WorkDto>>(token, "api/works");
        }
        public async Task<Author> PostAuthor(Author author)
        {
            // return await base.GetAsync<CodeEventDto>(token, $"api/events/{name}");
            return await base.PostAsync<Author>(path: $"api/authors/", author);
        }

        //public async Task<(string, object)> PostAuthor2(string str, Author author)
        //{
        //    // return await base.GetAsync<CodeEventDto>(token, $"api/events/{name}");
        //    return await base.PostAsync2<(string, object)>(path: $"api/authors/", ("", author));

        //}

        public async Task<string> PostAuthor2(Author author)
        {
            return await base.PostAsync2(path: $"api/authors/", author);
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
            //Microsoft HTTP client class has these already.
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
        //public async Task<string> PostAsync<T>(string path, T content, string contentType = "application/json") //Todo what to return
        public async Task<T> PostAsync<T>(string path, T content, string contentType = "application/json")
        {
            
            //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0
            //Request
            var request = new HttpRequestMessage(HttpMethod.Post, path);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

            //Respons
            string json = JsonConvert.SerializeObject(content);
            StringContent stringContent = new StringContent(json, System.Text.Encoding.UTF8, contentType);

            

            var response = await HttpClient.PostAsync(path, stringContent); //HttpResponseMessage
            if (response.IsSuccessStatusCode)
            {
                response.StatusCode.ToString();
                return content;

            }
            return content;
            //return (response.StatusCode.ToString());

        }
        //(string, T)
        //Todo Deconstruction of T is needed but how? (string, T) perhaps
        //public async Task<T> PostAsync2<T>(string path, T content, string contentType = "application/json") //where T:TestClass
        //{
        //    //Tuple Management

        //    //content = (TestClass)content;

        //    //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0
        //    //Request
        //    var request = new HttpRequestMessage(HttpMethod.Post, path);
        //    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

        //    //Respons
        //    string json = JsonConvert.SerializeObject(content);
        //    StringContent stringContent = new StringContent(json, System.Text.Encoding.UTF8, contentType);


        //    var response = await HttpClient.PostAsync(path, stringContent); //HttpResponseMessage
        //    if (response.IsSuccessStatusCode)
        //    {
        //        //return (response.StatusCode.ToString());
        //        return content;
        //    }
        //    return content;
        //    //return (response.StatusCode.ToString());

        //}

        public async Task<string> PostAsync2(string path, Author body, string contentType = "application/json") //where T:TestClass
        {
            //Tuple Management

            //content = (TestClass)content;

            //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0
            //Request
            var request = new HttpRequestMessage(HttpMethod.Post, path);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

            //Respons
            string json = JsonConvert.SerializeObject(body);
            StringContent stringContent = new StringContent(json, System.Text.Encoding.UTF8, contentType);


            var response = await HttpClient.PostAsync(path, stringContent); //HttpResponseMessage
            if (response.IsSuccessStatusCode)
            {
                return (response.StatusCode.ToString());
                //return content;
            }
            //return content;
            return (response.StatusCode.ToString());

        }




    }
}
