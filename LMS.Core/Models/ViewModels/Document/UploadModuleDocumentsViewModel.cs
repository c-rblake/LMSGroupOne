using LMS.Core.Models.Entities;
using LMS.Core.Validation;
using LMSGroupOne.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Models.ViewModels.Document
{
    public class UploadModuleDocumentsViewModel
    {
        public string Name { get; set; }
        [MaxLength(100, ErrorMessage = "Max length for document description is 250 characters")]
        [Display(Name = "Document description (optional)")]
        public string Description { get; set; }
        public string DocumentUrl { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PersonId { get; set; }
        public int ModuleId { get; set; }
        public int CourseId { get; set; }
        public Person Person { get; set; }
        public Entities.Module Module { get; set; }
        public Entities.Course Course { get; set; }

        [Display(Name = "Document(s) to upload")]
        [Required(ErrorMessage = "Please choose document(s) to upload")]
        [DataType(DataType.Upload)]
        [AllowedDocumentExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif", ".txt", ".doc", ".ppt", ".pdf", ".xd" })]
        [MaxDocumentSize(10 * 1024 * 1024)]
        public List<IFormFile> PostedDocuments { get; set; }
    }
}