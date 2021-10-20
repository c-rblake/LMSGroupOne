using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using AutoMapper;

namespace LMSGroupOne.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMapper mapper;
        public AuthorsController(IHttpClientFactory httpClientFactory, IMapper mapper)
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
            var response = await client.GetAsync("Authors");

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
