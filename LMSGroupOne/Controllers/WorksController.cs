using AutoMapper;
using LMS.Core.Models.Dto;
using LMS.Core.Models.Entities.API;
using LMS.Core.Models.ViewModels.API.Work;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LMSGroupOne.Controllers
{
    public class WorksController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMapper mapper;
        public WorksController(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            this.httpClientFactory = httpClientFactory;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> GetWorks()
        {
            var client = httpClientFactory.CreateClient("LMSClient");
            var response = await client.GetAsync("Works");

            //If success received   
            IEnumerable<WorkDto> works = default;
            if (response.IsSuccessStatusCode)
            {
                works = await response.Content.ReadAsAsync<IEnumerable<WorkDto>>();

            }
            else
            {
                //Error response received   
                //courses = Enumerable.Empty<CourseViewModel>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }

            //Map 
            var model = mapper.Map<IEnumerable<WorksViewModel>>(works);

            //return View(courses);
            //var model = courses
            // //.Where(p => p.Category == searchText || p.Name == searchText)
            // .Select(p => new CourseViewModel
            // {
            //     Title = p.Title,
            //     StartDate = p.StartDate
            // });
            return View("GetWorks", model.ToList());
        }
        public async Task<ActionResult> CreateWork([Bind("GenreId,TypeId,Title,Description,Level,PublicationDate")] Work work)
        {
            var client = httpClientFactory.CreateClient("LMSClient");
            JsonContent content = JsonContent.Create(work);

            var response = await client.PostAsync("Works", content);

            //If success received   
            WorkCreateDto workCreated = default;
            if (response.IsSuccessStatusCode)
            {
                workCreated = await response.Content.ReadAsAsync<WorkCreateDto>();

            }
            else
            {
                //Error response received   
                //courses = Enumerable.Empty<CourseViewModel>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }

            return RedirectToAction(nameof(GetWorks));
        }
        public IActionResult Create()
        {


            return View();
        }
    }
}
