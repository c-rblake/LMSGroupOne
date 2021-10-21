using AutoMapper;
using LMS.Core.Models.Entities;
using LMS.Core.Models.ViewModels.Account;
using LMS.Core.Repositories;
using LMSGroupOne.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            if (User.IsInRole("Teacher"))
            {
                return (View("IndexTeacher"));
            }

            return (View());

        }

        [Authorize(Roles = "Teacher")]
        [Route("/account/create/")]
        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        [Route("/account/create/")]
        public async Task<IActionResult> CreateAccount(AccountCreateViewModel newAccount)
        {
            if (ModelState.IsValid)
            {
                string email = newAccount.Email;

                var newAccountEmailExists = await uow.AccountRepository.EmailExists(email);

                if (newAccountEmailExists == true)
                {
                    ModelState.AddModelError(string.Empty, $"User '{email}' already exists");
                    return View();
                }

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

                ViewBag.UserName = person.Email;
                ViewBag.Role = role;
            }
            return View();
        }

        [Authorize(Roles = "Teacher")]
        [Route("/account/edit/{id}")]
        public async Task<IActionResult> EditAccount(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personAccount = await uow.AccountRepository.FindByIdAsync(id);

            if (personAccount == null)
            {
                return NotFound();
            }

            var userAccount = await uow.AccountRepository.GetUserAsync(id);

            if (userAccount == null)
            {
                return NotFound();
            }

            var accountRole = await uow.AccountRepository.RoleFindAsync(id);

            /*var viewModel = new AccountEditViewModel
            {
                Id = userAccount.Id,
                UserName = userAccount.UserName,
                FirstName = personAccount.FirstName,
                LastName = personAccount.LastName,
                Email = personAccount.Email,
                Role = accountRole.Name,
                CourseId = personAccount.CourseId
            };*/

            var account = mapper.Map<AccountEditViewModel>(await uow.AccountRepository.FindByIdAsync(id));

            if (account == null)
            {
                return NotFound();
            }


            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        [Route("/account/edit/{id}")]
        public async Task<IActionResult> EditAccount(AccountEditViewModel editAccount)
        {
            if (ModelState.IsValid)
            {
                var account = await uow.AccountRepository.FindByIdAsync(editAccount.Id);

                if (editAccount.Id != account.Id)
                {
                    return NotFound();
                }

                if (editAccount.Role == "Teacher")
                {
                    editAccount.CourseId = null;
                }

                account.UserName = editAccount.UserName;
                account.Email = editAccount.Email;
                account.FirstName = editAccount.FirstName;
                account.LastName = editAccount.LastName;
                account.CourseId = editAccount.CourseId;

                mapper.Map(editAccount, account);

                await uow.AccountRepository.UpdateRangePerson(account);
                await uow.CompleteAsync();

                var oldRole = await uow.AccountRepository.RoleFindAsync(editAccount.Id);

                var oldRoleName = oldRole.Name;

                var newRoleName = editAccount.Role;

                if (oldRoleName != newRoleName)
                {
                    await uow.AccountRepository.RoleUpdateAsync(account, oldRoleName, newRoleName);
                }

                ViewBag.UserName = account.Email;

            }
            return View();

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}