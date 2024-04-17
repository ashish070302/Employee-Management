using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Employee.UI.ViewModels.Account
{
    public class SignUpUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter Valid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter mobile number")]
        [Display(Name = "Mobile Number")]
		[StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be 10 digits")]
		public string Mobile { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter confirm password")]
        [Compare("Password", ErrorMessage = "confirm password can't matched!")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }
    }
}
