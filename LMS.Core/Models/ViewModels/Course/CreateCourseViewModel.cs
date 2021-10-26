using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Core.Models.ViewModels.Course
{
    public class CreateCourseViewModel
    {
        [Required]
        [MaxLength(100)]
        [Remote(action: "VerifyCourseName", controller: "Course")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]        
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }        
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }



        // 
        public bool Success { get; set; }  // creation status
        public string Message { get; set; }   // returnmessage
        public int ReturnId { get; set; }    // return id when created

    }
}
