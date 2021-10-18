using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Models.ViewModels.Document
{
    public class AddDocumentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter document name")]
        [MaxLength(40, ErrorMessage = "Max length for document name is 40 characters")]
        [Display(Name = "Document name")]
        public string Name { get; set; }
        [MaxLength(100, ErrorMessage = "Max length for document description is 250 characters")]
        [Display(Name = "Document description")]
        public string Description { get; set; }
        public string DocumentUrl { get; set; }
        public DateTime TimeStamp { get; set; }
        [Required]
        public string PersonId { get; set; }
        public int? ModuleId { get; set; }
        public int? ActivityId { get; set; }
        public int? CourseId { get; set; }
    }
}