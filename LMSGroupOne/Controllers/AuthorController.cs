using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using AutoMapper;
using LMS.Api.Core.Models;
using LMS.Api.Core.Dtos;

namespace LMSGroupOne.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMapper mapper;
        public AuthorController(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            this.httpClientFactory = httpClientFactory;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> GetAuthors()
        {
            var client = httpClientFactory.CreateClient("LMSClient");
            // client.BaseAddress = new Uri("https://localhost:44308/api/");

            //Called Course default GET All records  
            //GetAsync to send a GET request   
            // PutAsync to send a PUT request  
            var response = await client.GetAsync("Authors");
            //responseTask.Wait();

            //To store result of web api response.   
            // var result = responseTask.Result;

            //If success received   
            IEnumerable<AuthorDto> authors = default;
            if (response.IsSuccessStatusCode)
            {
                authors = await response.Content.ReadAsAsync<IEnumerable<AuthorDto>>();

            }
            else
            {
                //Error response received   
                //courses = Enumerable.Empty<CourseViewModel>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }

            //Map 
            var model = mapper.Map<IEnumerable<AuthorViewModel>>(authors);

            //return View(courses);
            //var model = courses
            // //.Where(p => p.Category == searchText || p.Name == searchText)
            // .Select(p => new CourseViewModel
            // {
            //     Title = p.Title,
            //     StartDate = p.StartDate
            // });

            return View("GetAuthors", model.ToList());
        }
    }
}
