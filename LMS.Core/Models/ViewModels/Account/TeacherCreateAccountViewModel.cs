using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Core.Models.ViewModels.Account
{
    public class TeacherCreateAccountViewModel
    {
        [Required(ErrorMessage = "Please enter first name")]
        [MaxLength(40, ErrorMessage = "Max length for first name is 40 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name")]
        [MaxLength(50, ErrorMessage = "Max length for last name is 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter an e-mail address")]
        [RegularExpression(@"^[\w+][\w\.\-]+@[\w\-]+(\.\w{2,4})+$|^\d{4}(\-)?\d{6}$|^91\-?\d{4}\-?\d{6}$", ErrorMessage = "Invalid format for e-mail address")]
        [Display(Name = "E-mail address (this will be the user name)")]
        public string Email { get; set; }

        //[UIHint("stringPassword")]
        [RegularExpression(@"[^<>]*", ErrorMessage = "The password format is incorrect")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }

        public int? CourseId { get; set; }

        public string Role { get; set; }
    }
}