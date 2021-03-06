using AutoMapper;
using LMS.Core.Models.Entities;
using LMS.Core.Models.ViewModels.Account;
using LMS.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LMSGroupOne.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public IActionResult Index()
        {
            return View();
        }

        public AccountController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Teacher")]
        [Route("/account/create/")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        [Route("/account/create/")]
        public async Task<IActionResult> Create(AccountCreateViewModel newAccount)
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

                string role = newAccount.Role;

                string password = "Hoppsan123!";

                if (role == "Teacher")
                {
                    password = "Hejsan123!";
                }

                await uow.AccountRepository.AddAccount(mapper.Map<Person>(person), password, role);
                await uow.CompleteAsync();

                ViewBag.UserName = person.Email;
                ViewBag.Role = role;
            }
            return View();
        }


        [Authorize(Roles = "Teacher")]
        public IActionResult CreateTeacher()
        {
            var model = new AccountCreateViewModel
            {

                Role = "Teacher"
            };


            return PartialView(model);
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult CreateStudent(int courseId)
        {
            var model = new AccountCreateViewModel
            {
                CourseId = courseId,
                Role = "Student",
            };

            return PartialView(new AccountCreateViewModel());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> CreateTeacher(AccountCreateViewModel newAccount)
        {
            newAccount.Role = "Teacher";
            newAccount.CourseId = null;

            if (ModelState.IsValid)
            {

                var person = new Person
                {

                    UserName = newAccount.Email,
                    Email = newAccount.Email,
                    FirstName = newAccount.FirstName,
                    LastName = newAccount.LastName,
                    CourseId = newAccount.CourseId,
                };
                string role = "Teacher";
                string password = "Hejsan123!";


                await uow.AccountRepository.AddAccount(person, password, role);
                //await uow.AccountRepository.AddAccount(mapper.Map<Person>(person), password, role);  
                uow.CompleteAsync().Wait();


                newAccount.PersonReturnId = person.Id;
                newAccount.ReturnId = 0;
                newAccount.Message = "Teacher created!";
                newAccount.Success = true;
                return PartialView(newAccount);


            }
            else
            {
                newAccount.Message = "Teacher not created!";
                newAccount.Success = false;
            }

            newAccount.Message = "Teacher not created!";
            newAccount.Success = false;
            return PartialView(newAccount);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> CreateStudent(AccountCreateViewModel newAccount)
        {
            newAccount.Role = "Student";
            newAccount.CourseId = null;

            if (ModelState.IsValid)
            {

                var person = new Person
                {

                    UserName = newAccount.Email,
                    Email = newAccount.Email,
                    FirstName = newAccount.FirstName,
                    LastName = newAccount.LastName,
                    CourseId = newAccount.CourseId,
                };
                string role = "Student";
                string password = "Hoppsan123!";


                await uow.AccountRepository.AddAccount(person, password, role);
                //await uow.AccountRepository.AddAccount(mapper.Map<Person>(person), password, role);  
                uow.CompleteAsync().Wait();


                newAccount.PersonReturnId = person.Id;
                newAccount.ReturnId = 0;
                newAccount.Message = "Student created!";
                newAccount.Success = true;
                return PartialView(newAccount);


            }
            else
            {
                newAccount.Message = "Student not created!";
                newAccount.Success = false;
            }

            newAccount.Message = "Student not created!";
            newAccount.Success = false;
            return PartialView(newAccount);
        }


        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accountRole = await uow.AccountRepository.RoleFindAsync(id);

            var account = mapper.Map<AccountEditViewModel>(await uow.AccountRepository.FindByIdAsync(id));

            account.Role = accountRole.Name;

            account.Id = id;

            if (account == null)
            {
                return NotFound();
            }

            return PartialView(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(AccountEditViewModel editAccount)
        {
            if (editAccount.Role == "Student" && editAccount.CourseId == null)
            {
                ModelState.AddModelError("Course", "Course is required for Student");
                return PartialView(editAccount);
            }

            if (editAccount.Role == "Teacher")
            {
                editAccount.CourseId = null;
            }

            if (ModelState.IsValid)
            {

                try
                {
                    var account = await uow.AccountRepository.FindByIdAsync(editAccount.Id);
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

                    editAccount.Success = true;
                    editAccount.PersonReturnId = editAccount.Id;
                    editAccount.Message = "Account was edited!";

                    return PartialView(editAccount);

                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }

            editAccount.Success = false;
            editAccount.PersonReturnId = editAccount.Id;
            editAccount.Message = "Could not edit account!";

            return PartialView(editAccount);
        }
    }
}


