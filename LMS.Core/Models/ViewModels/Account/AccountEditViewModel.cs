using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LMS.Core.Models.ViewModels.Account
{
    public class AccountEditViewModel:IModalViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required(ErrorMessage = "Please enter user name")]
        [MaxLength(100, ErrorMessage = "Max length for user name is 100 characters")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter first name")]
        [MaxLength(40, ErrorMessage = "Max length for first name is 40 characters")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name")]
        [MaxLength(50, ErrorMessage = "Max length for last name is 50 characters")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required, EmailAddress]
        [Display(Name = "E-mail address")]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }

        [Display(Name = "Course")]
        public int? CourseId { get; set; }


        public bool Success { get; set; }
        public string Message { get; set; }
        public int ReturnId { get; set; }
        public string PersonReturnId { get; set; }
    }
}