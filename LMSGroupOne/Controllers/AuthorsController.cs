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



        public IActionResult Search()
        {
            LibrarySearch model = new LibrarySearch
            {

            };

            return View(model);
        }

        public IActionResult Search(LibrarySearch model)
        {

            return View(model);
        }




        public IActionResult Index()
        {
            

            return View();
        }
        public async Task<ActionResult> GetAuthors()
        {
            var client = httpClientFactory.CreateClient("LMSClient");
            var response = await client.GetAsync("Authors/?includeWorks=true");

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
            var model = mapper.Map<IEnumerable<AuthorsViewmodel>>(authors);

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



        public async Task<IActionResult> List(LibrarySearch search)
        {
            var client = httpClientFactory.CreateClient("LMSClient");
            var response2 = await client.GetAsync($"works/?Title={search.Name}"); //Works is a more powerful search but we already have front end author code.
            //if (!string.IsNullOrWhiteSpace(workResourceParameters.Title)) if (!string.IsNullOrWhiteSpace(workResourceParameters.AuthorName)) Works Controller has both.
            var response = await client.GetAsync($"Authors/?includeWorks=true&Name={search.Name}");

            //   "Works/?Title={search.Name}

            //If success received   
            IEnumerable<AuthorDto> authors = default;
            IEnumerable<WorkDto> works = default;
            List<AuthorsViewmodel> reWorksAuthorsViewmodel = new();

            if (response.IsSuccessStatusCode)
            {
                authors = await response.Content.ReadAsAsync<IEnumerable<AuthorDto>>();
            }
            else
            {
                //Error response received   
                //courses = Enumerable.Empty<CourseViewModel>();
                ModelState.AddModelError(string.Empty, "Server error Authors.");
            }
            if (response2.IsSuccessStatusCode)
            {
                works = await response2.Content.ReadAsAsync<IEnumerable<WorkDto>>();
                foreach (var workDto in works)
                {
                    foreach (var workAuthorDto in workDto.Authors)
                    {
                        var author = new AuthorsViewmodel
                        {
                            Name = workAuthorDto.Name,
                            //Works = mapper.Map<Work>(workDto), //<= does not work well.
                            Age = workAuthorDto.Age,
                            Id = workAuthorDto.Id
                        };
                        var work = mapper.Map<Work>(workDto);
                        author.Works.Add(work);
                        reWorksAuthorsViewmodel.Add(author);
                    }
                }
            }
            else
            {
                //Error response received   
                //courses = Enumerable.Empty<CourseViewModel>();
                ModelState.AddModelError(string.Empty, "Server error Works.");
            }




            //Map 
            var model = mapper.Map<List<AuthorsViewmodel>>(authors);
            if (model is not null && reWorksAuthorsViewmodel is not null)
            {
                model.AddRange(reWorksAuthorsViewmodel);
            }
            if(search.OrderOnAge)
            {
                model = model.OrderBy(a => a.Age).ToList();
            }
            if (search.NameOrdered)
            {
                model = model.OrderBy(a => a.Name).ToList();
            }


            TempData["SearchTerm"] = search.Name;


                        

            return PartialView(model);
        }
       






        public async Task<ActionResult> GetAuthor(LibrarySearch search)
        {
            var client = httpClientFactory.CreateClient("LMSClient");
            var response2 = await client.GetAsync($"works/?Title={search.Name}"); //Works is a more powerful search but we already have front end author code.
            //if (!string.IsNullOrWhiteSpace(workResourceParameters.Title)) if (!string.IsNullOrWhiteSpace(workResourceParameters.AuthorName)) Works Controller has both.
            var response = await client.GetAsync($"Authors/?includeWorks=true&Name={search.Name}");
 
            //   "Works/?Title={search.Name}

            //If success received   
            IEnumerable<AuthorDto> authors = default;
            IEnumerable<WorkDto> works = default;
            List<AuthorsViewmodel> reWorksAuthorsViewmodel = new();

            if (response.IsSuccessStatusCode)
            {
                authors = await response.Content.ReadAsAsync<IEnumerable<AuthorDto>>();
            }
            else
            {
                //Error response received   
                //courses = Enumerable.Empty<CourseViewModel>();
                ModelState.AddModelError(string.Empty, "Server error Authors.");
            }
            if(response2.IsSuccessStatusCode)
            {
                works = await response2.Content.ReadAsAsync<IEnumerable<WorkDto>>();
                foreach (var workDto in works)
                {
                    foreach (var workAuthorDto in workDto.Authors)
                    {
                        var author = new AuthorsViewmodel
                        {
                            Name = workAuthorDto.Name,
                            //Works = mapper.Map<Work>(workDto), //<= does not work well.
                            Age = workAuthorDto.Age,
                            Id= workAuthorDto.Id
                        };
                        var work = mapper.Map<Work>(workDto);
                        author.Works.Add(work);
                        reWorksAuthorsViewmodel.Add(author);
                    }
                }
            }
            else
            {
                //Error response received   
                //courses = Enumerable.Empty<CourseViewModel>();
                ModelState.AddModelError(string.Empty, "Server error Works.");
            }




            //Map 
            var model = mapper.Map<List<AuthorsViewmodel>>(authors);
            if (model is not null && reWorksAuthorsViewmodel is not null)
            {
                model.AddRange(reWorksAuthorsViewmodel);
            }

                //return View(courses);
                //var model = courses
                // //.Where(p => p.Category == searchText || p.Name == searchText)
                // .Select(p => new CourseViewModel
                // {
                //     Title = p.Title,
                //     StartDate = p.StartDate
                // });

                //return View("GetAuthors", model.ToList());
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
            new AuthorCreateViewModel
            {
                DateOfBirth = DateTime.Now,
                DateOfDeath= DateTime.Now,
            };

            return PartialView(new AuthorCreateViewModel());
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Author author)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var auth = await CreateAuthor(author);
            
        //        return RedirectToAction(nameof(GetAuthors));
        //    }
        //    return PartialView(author);
        //}




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AuthorCreateViewModel model)
        {
           
            if (ModelState.IsValid)
            {

                try
                {
                    Author author = new Author
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        DateOfBirth = model.DateOfBirth,
                        DateOfDeath = model.DateOfDeath

                    };

                    CreateAuthor(author).Wait();
                    model.Success = true;
                    model.Message = "Author created!";
                    model.ReturnId = author.Id;
                    return PartialView(model);

                }
                catch(Exception e)
                {
                    throw;
                }


                



            }
            
           


            model.Success = false;
            model.ReturnId = 0;
            model.Message = "Could not create author!";            
            return PartialView(model);
        }


    }
}
//[Bind("FirstName,LastName,DateOfBirth,DateOfDeath")]