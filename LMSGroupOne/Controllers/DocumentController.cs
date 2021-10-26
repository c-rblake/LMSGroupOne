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
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _db;

        public DocumentController(ILogger<DocumentController> logger, UserManager<Person> userManager, IWebHostEnvironment environment, ApplicationDbContext db)
        {
            _logger = logger;
            _userManager = userManager;
            _environment = environment;
            _db = db;
        }


        [Authorize(Roles = "Teacher")]
        [Route("/document/uploadcoursedocuments/{id}")]
        public async Task<IActionResult> UploadCourseDocuments(int id)
        {
            var course = await _db.Courses.Where(c => c.Id == id).FirstOrDefaultAsync();

            var viewModel = new UploadCourseDocumentsViewModel
            {
                Course = course
            };

            return View(viewModel);
        }


        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        [Route("/document/uploadcoursedocuments/{id}")]
        public async Task<IActionResult> UploadCourseDocuments(int id, List<IFormFile> postedDocuments, UploadCourseDocumentsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var course = await _db.Courses.Where(a => a.Id == id).FirstOrDefaultAsync();

                string path = Path.Combine(_environment.WebRootPath, $"documents/{course.Name}");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var userId = _userManager.GetUserId(User);

                var person = await _db.Persons.Where(p => p.Id == userId).FirstOrDefaultAsync();

                try
                {
                    foreach (IFormFile postedDocument in postedDocuments)
                    {
                        if (postedDocument == null || postedDocument.Length == 0)
                        {
                            ModelState.AddModelError("Document", $"Document {postedDocument.FileName} is empty or null");
                            return View();
                        }

                        string documentName = Path.GetFileName(postedDocument.FileName);

                        string documentUrl = Path.Combine(path, documentName);

                        if (DocumentExists(documentName.Split(".")[0]) == true)
                        {
                            ModelState.AddModelError("Document", $"A document with file name {postedDocument.FileName} already exists");
                            return View();
                        }

                        using (FileStream stream = new FileStream(documentUrl, FileMode.Create))
                        {
                            await postedDocument.CopyToAsync(stream);
                        }

                        var document = new Document
                        {
                            Name = documentName.Split(".")[0],
                            DocumentUrl = documentUrl,
                            TimeStamp = DateTime.Now,
                            Person = person,
                            Course = course,
                            //PersonId = userId,
                            //CourseId = course.Id,
                            Description = viewModel.Description
                        };

                        _db.Documents.AddRange(document);
                        await _db.SaveChangesAsync();
                    }

                    long size = postedDocuments.Sum(f => f.Length);

                    ViewBag.Result = $"{postedDocuments.Count} document(s) with total size {size} bytes was successfully uploaded";
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return View();
            }
            return View();
        }

        [Authorize(Roles = "Teacher")]
        [Route("/document/uploadmoduledocuments/{id}")]
        public async Task<IActionResult> UploadModuleDocuments(int id)
        {
            var module = await _db.Modules.Where(a => a.Id == id).FirstOrDefaultAsync();

            var parentCourse = await _db.Courses.Where(c => c.Id == module.CourseId).FirstOrDefaultAsync();

            var viewModel = new UploadModuleDocumentsViewModel
            {
                Module = module,
                Course = parentCourse
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        [Route("/document/uploadmoduledocuments/{id}")]
        public async Task<IActionResult> UploadModuleDocuments(int id, List<IFormFile> postedDocuments, UploadModuleDocumentsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var module = await _db.Modules.Where(a => a.Id == id).FirstOrDefaultAsync();

                var parentCourse = await _db.Courses.Where(c => c.Id == module.CourseId).FirstOrDefaultAsync();

                string path = Path.Combine(_environment.WebRootPath, $"documents/{parentCourse.Name}/{module.Name}");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var userId = _userManager.GetUserId(User);

                var person = await _db.Persons.Where(p => p.Id == userId).FirstOrDefaultAsync();

                try
                {
                    foreach (IFormFile postedDocument in postedDocuments)
                    {
                        string documentName = Path.GetFileName(postedDocument.FileName);

                        string documentUrl = Path.Combine(path, documentName);

                        if (DocumentExists(documentName.Split(".")[0]) == true)
                        {
                            ModelState.AddModelError("Document", $"A document with file name {postedDocument.FileName} already exists");
                            return View();
                        }

                        using (FileStream stream = new FileStream(documentUrl, FileMode.Create))
                        {
                            await postedDocument.CopyToAsync(stream);
                        }

                        var document = new Document
                        {
                            Name = documentName.Split(".")[0],
                            DocumentUrl = documentUrl,
                            TimeStamp = DateTime.Now,
                            Person = person,
                            Module = module,
                            Course = parentCourse,
                            //PersonId = userId,
                            //ModuleId = module.Id,
                            //CourseId = parentCourse.Id,
                            Description = viewModel.Description
                        };

                        _db.Documents.AddRange(document);
                        await _db.SaveChangesAsync();
                    }

                    long size = postedDocuments.Sum(f => f.Length);

                    ViewBag.Result = $"{postedDocuments.Count} document(s) with total size {size} bytes was successfully uploaded";
                }

                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return View();
            }
            return View();
        }

        [Authorize]
        [Route("/document/uploadactivitydocuments/{id}")]
        public async Task<IActionResult> UploadActivityDocuments(int id)
        {
            var activity = await _db.Activities.Where(a => a.Id == id).FirstOrDefaultAsync();

            var parentModule = await _db.Modules.Where(m => m.Id == activity.ModuleId).FirstOrDefaultAsync();

            var parentCourse = await _db.Courses.Where(c => c.Id == parentModule.CourseId).FirstOrDefaultAsync();

            var viewModel = new UploadActivityDocumentsViewModel
            {
                Activity = activity,
                Module = parentModule,
                Course = parentCourse
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [Route("/document/uploadactivitydocuments/{id}")]
        public async Task<IActionResult> UploadActivityDocuments(int id, List<IFormFile> postedDocuments, UploadActivityDocumentsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var activity = await _db.Activities.Where(a => a.Id == id).FirstOrDefaultAsync();

                var activityType = _db.ActivityTypes.Where(a => a.Id == activity.ActivityTypeId).FirstOrDefault();

                if (User.IsInRole("Student") && activityType.Name != "Assignment")
                {
                    ModelState.AddModelError("ActivityType", "Students cannot upload documents to activities that are not assignments");
                    return View(viewModel);
                }

                var parentModule = _db.Modules.Where(c => c.Id == activity.ModuleId).FirstOrDefault();

                var parentCourse = _db.Courses.Where(c => c.Id == parentModule.CourseId).FirstOrDefault();

                string path = Path.Combine(_environment.WebRootPath, $"documents/{parentCourse.Name}/{parentModule.Name}/{activity.Name}");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var userId = _userManager.GetUserId(User);

                var person = await _db.Persons.Where(p => p.Id == userId).FirstOrDefaultAsync();

                try
                {
                    foreach (IFormFile postedDocument in postedDocuments)
                    {
                        string documentName = Path.GetFileName(postedDocument.FileName);

                        string documentUrl = Path.Combine(path, documentName);

                        if (DocumentExists(documentUrl) == true)
                        {
                            ModelState.AddModelError("Document", $"A document with file name {postedDocument.FileName} already exists in this location");
                            return View();
                        }

                        using (FileStream stream = new FileStream(documentUrl, FileMode.Create))
                        {
                            await postedDocument.CopyToAsync(stream);
                        }

                        var document = new Document
                        {
                            Name = documentName.Split(".")[0],
                            DocumentUrl = documentUrl,
                            TimeStamp = DateTime.Now,
                            Person = person,
                            Activity = activity,
                            Module = parentModule,
                            Course = parentCourse,
                            Description = viewModel.Description
                        };

                        _db.Documents.AddRange(document);
                        await _db.SaveChangesAsync();
                    }

                    long size = postedDocuments.Sum(f => f.Length);

                    ViewBag.Result = $"{postedDocuments.Count} document(s) with total size {size} bytes was successfully uploaded";
                }

                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return View();
            }
            return View();
        }

        public bool DocumentExists(string documentUrl)
        {
            return _db.Documents.Any(d => d.DocumentUrl == documentUrl);
        }
    }
}