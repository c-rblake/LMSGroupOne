using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Models.ViewModels.Account
{
    public class AccountCreateViewModel:IModalViewModel
    {
        [Required(ErrorMessage = "Please enter first name")]
        [MaxLength(40, ErrorMessage = "Max length for first name is 40 characters")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name")]
        [MaxLength(50, ErrorMessage = "Max length for last name is 50 characters")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter an e-mail address"), EmailAddress]
        [Display(Name = "E-mail address (this will be the user name)")]
        public string Email { get; set; }

        //[UIHint("stringPassword")]
        [Required(ErrorMessage = "Please enter a password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long", MinimumLength = 6)]
        [RegularExpression(@"[^<>]*", ErrorMessage = "The password format is incorrect")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please choose a role")]
        public string Role { get; set; }

        [Display(Name = "Course")]
        public int? CourseId { get; set; }


        public bool Success { get; set; }
        public string Message { get; set; }
        public int ReturnId { get; set; }
        public string PersonReturnId { get; set; }
    }
}