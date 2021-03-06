using AutoMapper;
using LMS.Core.Repositories;
using LMSGroupOne.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LMSGroupOne.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork uow, IMapper mapper)
        {
            _logger = logger;
            this.uow = uow;
            this.mapper = mapper;
        }

        [Authorize]
        public IActionResult Index()
        {
            //if (User.IsInRole("Teacher"))
            //{
            //    return (View("IndexTeacher"));
            //}

            return RedirectToAction("Index", "MainNavigation");

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}