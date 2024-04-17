using System.ComponentModel.DataAnnotations;

namespace Employee.UI.ViewModels.Account
{
    public class LoginSignUpViewModel
    {
		[Required]
		public string Username { get; set; }
		[Required]
		public string Password { get; set; }

		[Display(Name = "Remember Me")]
		public bool IsRemember { get; set; }

	}
}
