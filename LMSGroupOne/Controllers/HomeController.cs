using LMS.Core.Models.Entities;
using LMSGroupOne.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using LMS.Core.Models.ViewModels.Document;

namespace LMSGroupOne.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly IWebHostEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, UserManager<Person> userManager, SignInManager<Person> signInManager, IWebHostEnvironment environment)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
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

        [Authorize]
        public IActionResult UploadDocuments()
        {
            var addDocumentViewModel = new AddDocumentViewModel { };

            return View(addDocumentViewModel);
        }


        [HttpPost]
        [Authorize]
        public IActionResult UploadDocuments(List<IFormFile> postedDocuments, AddDocumentViewModel viewModel)
        {
            string wwwPath = this._environment.WebRootPath;

            string documentsDirectoryPath = Path.Combine(wwwPath, "documents");

            if (!Directory.Exists(documentsDirectoryPath))
            {
                Directory.CreateDirectory(documentsDirectoryPath);
            }

            List<string> uploadedDocuments = new List<string>();

            foreach (IFormFile postedDocument in postedDocuments)
            {
                string fileName = Path.GetFileName(postedDocument.FileName);

                string documentUrl = Path.Combine(documentsDirectoryPath, fileName);

                var personId = _userManager.GetUserId(User);

                using (FileStream stream = new FileStream(documentUrl, FileMode.Create))
                {
                    postedDocument.CopyTo(stream);
                    uploadedDocuments.Add(fileName);
                    var document = new Document
                    {
                        Name = fileName,
                        DocumentUrl = documentUrl,
                        TimeStamp = DateTime.Now,
                        PersonId = personId
                    };

                    //db.Documents.Add(document);
                    //await db.SaveChangesAsync();

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