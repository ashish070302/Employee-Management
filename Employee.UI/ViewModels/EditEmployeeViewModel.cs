using System.ComponentModel.DataAnnotations;

namespace Employee.UI.ViewModels
{
    public class EditEmployeeViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        [Required(ErrorMessage = "Date of Birth is should be Greater than 18 years.")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DateOfBirthValidation]
        public DateTime DateOfBirth { get; set; }
        [DataType(DataType.Date)] 
        public DateTime DateOfJoining { get; set; }
        public string Aadhar { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImageURL { get; set; }
        public IFormFile? ChooseImage {  get; set; }
        public int DepartmentId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
    }
}
