using Employee.UI.ViewModels.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EmployeeEntity;
using EmployeeRepositories;
using System.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace Employee.UI.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext context;

        public AccountController(ApplicationDbContext _context)
        {
            context = _context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(SignUpUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = context.Users.FirstOrDefault(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "The username is already taken. Choose a new Username.");
                    return View(model);
                }

                var emailConfirmationToken = Guid.NewGuid().ToString(); // Generate a unique token for email confirmation

                var data = new User()
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = EncryptPassword(model.Password),
                    Mobile = model.Mobile,
                    IsActive = model.IsActive,
                    EmailConfirmationToken = emailConfirmationToken // Store the token
                };
                context.Users.Add(data);
                context.SaveChanges();

                SendEmailConfirmationEmail(model.Email, emailConfirmationToken);


                TempData["successMessage"] = "Account created successfully. Please check your email to confirm your email address.";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["errorMessage"] = "Empty Form cannot be submitted.";
                return View(model);
            }
        }

        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginSignUpViewModel model)
        {
			if (ModelState.IsValid)
			{
				var data = context.Users.Where(e => e.Username == model.Username).SingleOrDefault();
				if (data != null)
				{
                    if (!data.EmailConfirmed)
                    {
                        TempData["errorEmail"] = "Email not confirmed. Please check your email to confirm your email address.";
                        return View(model);
                    }

                    bool isValid = (data.Username == model.Username && DecryptPassword(data.Password) == model.Password);
					if (isValid)
					{
						var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, model.Username) }, CookieAuthenticationDefaults.AuthenticationScheme);
						var principal = new ClaimsPrincipal(identity);
						HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
						HttpContext.Session.SetString("Username", model.Username);

                        HttpContext.Session.SetString("Email", data.Email);
                        return RedirectToAction("Index", "Home");
					}
					else
					{
						TempData["errorPassword"] = "Invalid password!";
						return View(model);
					}
				}
				else
				{
					TempData["errorUsername"] = "Username not found!";
					return View(model);
				}
			}
			else
			{
				return View(model);
			}
		}

		public IActionResult LogOut()
		{
            // Retrieve the username from session
            var username = HttpContext.Session.GetString("Username");

            // Retrieve the user's email from session
            var userEmail = HttpContext.Session.GetString("Email");

            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string senderEmail = "awwshish1223@gmail.com";
            string senderPassword = "nvzs tfkk dtnr boig";

            // Send email to the user
            if (!string.IsNullOrEmpty(userEmail))
            {
                SendLogoutEmail(userEmail, username, smtpServer, smtpPort, senderEmail, senderPassword);
            }

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			var storedCookies = Request.Cookies.Keys;
			foreach (var cookies in storedCookies)
			{
				Response.Cookies.Delete(cookies);
			}
			return RedirectToAction("Login", "Account");
		}

        public static string EncryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }
            else
            {
                byte[] storePassword = ASCIIEncoding.ASCII.GetBytes(password);
                string encryptedPassword = Convert.ToBase64String(storePassword);
                return encryptedPassword;
            }
        }

        public static string DecryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }
            else
            {
                byte[] encryptedPassword = Convert.FromBase64String(password);
                string decryptedPassword = ASCIIEncoding.ASCII.GetString(encryptedPassword);
                return decryptedPassword;
            }
        }

        private void SendLogoutEmail(string userEmail, string username, string smtpServer, int smtpPort, string senderEmail, string senderPassword)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Employee Portal", senderEmail));
            message.To.Add(new MailboxAddress("", userEmail));
            message.Subject = "You are getting logged out";

            // Construct the email body with username
            message.Body = new TextPart("plain")
            {
                Text = $"Hey {username},\n\nHope you had a great experience on our portal. You are getting logged out now. Thank you for using our portal. \n\nBest Regards, \nEmployee Portal "
            };

            try
            {
                using (var client = new SmtpClient())
                {
                    // Connect to the SMTP server
                    client.Connect(smtpServer, smtpPort);

                    // Authenticate with sender credentials
                    client.Authenticate(senderEmail, senderPassword);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }

        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email && u.EmailConfirmationToken == token);
            if (user != null)
            {
                user.EmailConfirmed = true;
                user.EmailConfirmationToken = "";
                context.SaveChanges();
                TempData["successMessage"] = "Email confirmed successfully. You can now log in.";
            }
            else
            {
                TempData["errorMessage"] = "Invalid email confirmation link.";
            }

            return RedirectToAction("Login");
        }


        private void SendEmailConfirmationEmail(string userEmail, string emailConfirmationToken)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Employee Portal", "awwshish1223@gmail.com"));
            message.To.Add(new MailboxAddress("", userEmail));
            message.Subject = "Confirm your email address";

            // Construct the confirmation link with email and token
            var confirmationLink = Url.Action("ConfirmEmail", "Account", new { email = userEmail, token = emailConfirmationToken }, Request.Scheme);

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<p>Please click <a href='{confirmationLink}'>here</a> to confirm your email address.</p>";
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("awwshish1223@gmail.com", "nvzs tfkk dtnr boig");
                client.Send(message);
                client.Disconnect(true);
            }
        }

    }
}
