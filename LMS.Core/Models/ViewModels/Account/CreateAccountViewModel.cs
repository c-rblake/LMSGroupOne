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

        [Required(ErrorMessage = "Please enter an e-mail address"), EmailAddress]
        [Display(Name = "E-mail address (this will be the user name)")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please choose a role")]
        public string Role { get; set; }

        [Display(Name = "Add student to course")]
        public int? CourseId { get; set; }
    }
}