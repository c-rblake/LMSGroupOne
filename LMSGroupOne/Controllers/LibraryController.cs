using LMSGroupOne.LibraryClientApi;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LMSGroupOne.Controllers
{
    public class LibraryController:Controller
    {
        private readonly LibraryClient libraryClient;

        public LibraryController(LibraryClient libraryClient) //Todo dependency goes here.
        {
            this.libraryClient = libraryClient;
        }

        public async Task<IActionResult> IndexAsync() //Testmethods
        {
            var res = await GetWithStreamAndFactory();
            return View();
        }

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

                using(var streamReader = new StreamReader(stream)) //read stream
                {
                    using(var jsonReader = new JsonTextReader(streamReader)) // NewtonSoft to read json
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
}
