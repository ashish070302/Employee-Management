using System.ComponentModel.DataAnnotations;

namespace Employee.UI.ViewModels
{
    public class CreateEmployeeViewModel
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Address { get; set; }

        [Required(ErrorMessage = "Date of Birth is should be Greater than 18 years.")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DateOfBirthValidation]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfJoining { get; set; }

        [Required(ErrorMessage = "Aadhar number is required")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Aadhar number must be 12 digits")]
        public string Aadhar { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be 10 digits")]
        public string PhoneNumber { get; set; }

        [RegularExpression(@"^[\w\-\.]+@([\w-]+\.)+[\w-]{2,}$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        public IFormFile ImageURL { get; set; }
        public int DepartmentId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public List<StateViewModel> States { get; set; }
    }
    public class DateOfBirthValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime dateOfBirth = (DateTime)value;
                if (DateTime.Today.AddYears(-18) < dateOfBirth)
                {
                    return new ValidationResult("Date of birth must be at least 18 years ago.");
                }
            }
            return ValidationResult.Success;
        }
    }
}

