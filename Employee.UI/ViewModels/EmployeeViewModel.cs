using Employee.UI.Utility;
using EmployeeEntity;
using System.ComponentModel.DataAnnotations;

namespace Employee.UI.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DateOfBirthValidation]
        public DateTime DateOfBirth { get; set; }
        [DataType(DataType.Date)] 
        public DateTime DateOfJoining { get; set; }
        public string Aadhar { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImageURL { get; set; }
        public string DeptartmentName { get; set; }
        public string StateName { get; set; }
        public string CityName {  get; set; }
    }
    public class PageEmployeeViewModel
    {
        public List<EmployeeViewModel> Employee { get; set; }
        public PageInfo PageInfo { get; set; }
    }

}
