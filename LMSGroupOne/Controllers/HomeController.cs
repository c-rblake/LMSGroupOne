using AutoMapper;
using LMS.Core.Models.Entities;
using LMS.Core.Models.ViewModels.Account;
using LMS.Core.Repositories;
using LMSGroupOne.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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

        [Authorize(Roles = "Teacher")]
        public IActionResult CreateAccount()
        {
            var createAccountViewModel = new CreateAccountViewModel { };

            return View(createAccountViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> CreateAccount(CreateAccountViewModel newAccount)
        {
            if (ModelState.IsValid)
            {
                if (newAccount.Role == "Teacher")
                {
                    newAccount.CourseId = null;
                }

                var person = new Person
                {
                    UserName = newAccount.Email,
                    Email = newAccount.Email,
                    FirstName = newAccount.FirstName,
                    LastName = newAccount.LastName,
                    CourseId = newAccount.CourseId,
                };

                string password = newAccount.Password;
                string role = newAccount.Role;

                await uow.AccountRepository.AddAccount(mapper.Map<Person>(person), password, role);
                await uow.CompleteAsync();
            }
            return View();
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