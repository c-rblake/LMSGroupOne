using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using AutoMapper;
using LMS.Core.Models.Dto;
using LMS.Core.Models.ViewModels;
using LMS.Core.Models.ViewModels.API;
using LMS.Core.Models.Entities.API;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using LMS.Core.Models.ViewModels.API.Author;

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
        public async Task<ActionResult> GetAuthorAndWorks(int? id)
        {
            var client = httpClientFactory.CreateClient("LMSClient");
            var response = await client.GetAsync("authors/"+ id + "?includeWorks=true");

            //If success received   
           WorkAuthorDto authorWorks = default;
            if (response.IsSuccessStatusCode)
            {
                authorWorks = await response.Content.ReadAsAsync<WorkAuthorDto>();

            }
            else
            {
                //Error response received   
                //courses = Enumerable.Empty<CourseViewModel>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }

            //Map 
            var model = mapper.Map<AuthorWorksViewModel>(authorWorks);

            //return View(courses);
            //var model = courses
            // //.Where(p => p.Category == searchText || p.Name == searchText)
            // .Select(p => new CourseViewModel
            // {
            //     Title = p.Title,
            //     StartDate = p.StartDate
            // });

            return View("GetAuthorAndWorks", model);
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
            var model = mapper.Map<IEnumerable<AuthorsViewModel>>(authors);

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
        public async Task<ActionResult> CreateAuthor(Author author)
        {
            var client = httpClientFactory.CreateClient("LMSClient");
            JsonContent content = JsonContent.Create(author);
            
            var response = await client.PostAsync("Authors", content);

            //If success received   
            AuthorCreateDto authorCreated = default;
            if (response.IsSuccessStatusCode)
            {
                authorCreated = await response.Content.ReadAsAsync<AuthorCreateDto>();

            }
            else
            {
                //Error response received   
                //courses = Enumerable.Empty<CourseViewModel>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }

            return RedirectToAction(nameof(GetAuthors));
        }
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,DateOfBirth,DateOfDeath")] Author author)
        {
            if (ModelState.IsValid)
            {
                var auth = await CreateAuthor(author);
            
                return RedirectToAction(nameof(GetAuthors));
            }
            return View(author);
        }
    }
}
