using LMS.Core.Models.Entities;
using LMS.Core.Models.ViewModels.Document;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LMSGroupOne.Controllers
{

    public class DocumentController : Controller
    {
        private readonly ILogger<DocumentController> _logger;
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly IWebHostEnvironment _environment;

        public DocumentController(ILogger<DocumentController> logger, UserManager<Person> userManager, SignInManager<Person> signInManager, IWebHostEnvironment environment)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
        }


        [Authorize]
        public IActionResult Upload()
        {
            var addDocumentViewModel = new UploadDocumentsViewModel { };

            return View(addDocumentViewModel);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(List<IFormFile> postedDocuments, UploadDocumentsViewModel viewModel)
        {
            string wwwPath = _environment.WebRootPath;

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

    }
}
