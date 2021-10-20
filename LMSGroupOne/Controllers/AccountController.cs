using AutoMapper;
using LMS.Core.Models.Entities;
using LMS.Core.Models.ViewModels.Account;
using LMS.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> Create(CreateAccountViewModel newAccount)
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

    }
}