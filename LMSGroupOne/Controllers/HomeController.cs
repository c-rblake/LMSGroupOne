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
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public HomeController(ILogger<HomeController> logger, UserManager<Person> userManager, SignInManager<Person> signInManager, IUnitOfWork uow, IMapper mapper)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            this.uow = uow;
            this.mapper = mapper;
        }

        [Authorize]
        public IActionResult Index()
        {
            if (User.IsInRole("Teacher"))
            {
                return (View("IndexTeacher"));
            }

            return (View());

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
        public async Task<IActionResult> CreateAccount(CreateAccountViewModel viewModel, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = new Person
                {
                    UserName = viewModel.Email,
                    Email = viewModel.Email,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    CourseId = viewModel.CourseId
                };

                //uow.AccountRepository.AddAccount(mapper.Map<CreateAccountViewModel>(user));
                //await uow.CompleteAsync();

                var result1 = await _userManager.CreateAsync(user, viewModel.Password);
                var result2 = await _userManager.AddToRoleAsync(user, viewModel.Role);

                if (result1.Succeeded && result2.Succeeded)
                {
                    _logger.LogInformation("Account was created with password and role");
                }

                foreach (var error in result1.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                foreach (var error in result2.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            return RedirectToAction(nameof(Index), "Home");
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