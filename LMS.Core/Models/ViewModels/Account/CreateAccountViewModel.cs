using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Models.ViewModels.Account
{
    public class CreateAccountViewModel
    {
        [Required(ErrorMessage = "Please enter first name")]
        [MaxLength(40, ErrorMessage = "Max length for first name is 40 characters")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name")]
        [MaxLength(50, ErrorMessage = "Max length for last name is 50 characters")]
        [Display(Name = "Last name")]

        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter an e-mail address")]
        [RegularExpression(@"^[\w+][\w\.\-]+@[\w\-]+(\.\w{2,4})+$|^\d{4}(\-)?\d{6}$|^91\-?\d{4}\-?\d{6}$", ErrorMessage = "Invalid format for e-mail address")]
        [Display(Name = "E-mail address (this will be the user name)")]
        public string Email { get; set; }

        //[UIHint("stringPassword")]
        [Required(ErrorMessage = "Please enter a password")]
        [RegularExpression(@"[^<>]*", ErrorMessage = "The password format is incorrect")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }

        public int? CourseId { get; set; }

        public string Role { get; set; }
    }
}