using LMS.Core.Models.Entities;
using LMS.Core.Models.ViewModels.Document;
using LMS.Data.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ApplicationDbContext _db;

        public DocumentController(ILogger<DocumentController> logger, UserManager<Person> userManager, SignInManager<Person> signInManager, IWebHostEnvironment environment, ApplicationDbContext db)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
            _db = db;
        }


        [Authorize]
        [Route("/document/uploadcoursedocuments/{id}")]
        public async Task<IActionResult> UploadCourseDocuments(int id)
        {
            var courses = await _db.Courses.ToListAsync();

            var course = courses.Where(a => a.Id == id).FirstOrDefault();

            var viewModel = new UploadDocumentsViewModel
            {
                Course = course
            };

            return View(viewModel);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("/document/uploadcoursedocuments/{id}")]
        public async Task<IActionResult> UploadCourseDocuments(int id, List<IFormFile> postedDocuments, UploadDocumentsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var course = _db.Courses.Where(a => a.Id == id).FirstOrDefault();

                string path = Path.Combine(_environment.WebRootPath, $"documents/{course.Name}");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var userId = _userManager.GetUserId(User);

                try
                {
                    foreach (IFormFile postedDocument in postedDocuments)
                    {
                        string fileName = Path.GetFileName(postedDocument.FileName);

                        string documentUrl = Path.Combine(path, fileName);

                        using (FileStream stream = new FileStream(documentUrl, FileMode.Create))
                        {
                            await postedDocument.CopyToAsync(stream);
                        }

                        var document = new Document
                        {
                            Name = fileName.Split(".")[0],
                            DocumentUrl = documentUrl,
                            TimeStamp = DateTime.Now,
                            PersonId = userId,
                            Course = course,
                            CourseId = course.Id
                        };

                        _db.Documents.AddRange(document);
                        await _db.SaveChangesAsync();
                    }

                    ViewBag.Result = "Upload was successfull";
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return View();
            }
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}