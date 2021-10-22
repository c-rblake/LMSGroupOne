using LMS.Core.Models.Entities;
using LMS.Core.Models.ViewModels.Document;
using LMS.Data.Data;
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
        public IActionResult Upload()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(List<IFormFile> postedDocuments, UploadDocumentsViewModel viewModel, string description)
        {
                string wwwPath = _environment.WebRootPath;

                string documentsDirectoryPath = Path.Combine(wwwPath, "documents");

                if (!Directory.Exists(documentsDirectoryPath))
                {
                    Directory.CreateDirectory(documentsDirectoryPath);
                }

                var userId = _userManager.GetUserId(User);

                foreach (IFormFile postedDocument in postedDocuments)
                {
                    string fileName = Path.GetFileName(postedDocument.FileName);

                    string documentUrl = Path.Combine(documentsDirectoryPath, fileName);

                    using (FileStream stream = new FileStream(documentUrl, FileMode.Create))
                    {
                        postedDocument.CopyTo(stream);
                    }
                                        
                    var document = new Document
                        {
                            Name = fileName,
                            DocumentUrl = documentUrl,
                            TimeStamp = DateTime.Now,
                            PersonId = userId,
                            Description = description
                        };

                     _db.Documents.AddRange(document);
                     await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index), "Home");
        }

        public async Task<IActionResult> Download()
        {
            var model = _db.Documents.Select(row => row);

            return View(nameof(Download), model);
        }

        public async Task<IActionResult> DownloadFile(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var file = _db.Documents.FirstOrDefault(d => d.Id == id);
            string filePath = file.DocumentUrl;

            string contentType = "application/pdf";

            byte[] fileBytes = GetFile(filePath);

            return File(
        fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filePath);


            //return View(DownloadFile);

        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }

        //public FileResult Download(string id)
        //{
        //    int fid = Convert.ToInt32(id);
        //    var files = objData.GetFiles();
        //    string filename = (from f in files
        //                       where f.FileId == fid
        //                       select f.FilePath).First();
        //    string contentType = "application/pdf";
        //    //Parameters to file are
        //    //1. The File Path on the File Server
        //    //2. The content type MIME type
        //    //3. The parameter for the file save by the browser
        //    return File(filename, contentType, "Report.pdf");
        //}
    }
}
