using System;
using System.ComponentModel.DataAnnotations;
using LMS.Core.Models.Entities;

namespace LMS.Core.Models.ViewModels.Document
{
    public class UploadDocumentsViewModel
    {
        public string Name { get; set; }
        [MaxLength(100, ErrorMessage = "Max length for document description is 250 characters")]
        [Display(Name = "Document description (optional)")]
        public string Description { get; set; }
        public string DocumentUrl { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PersonId { get; set; }
        public int? CourseId { get; set; }
        public Person Person { get; set; }
        public Entities.Course Course { get; set; }
    }
}