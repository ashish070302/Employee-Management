using System.ComponentModel.DataAnnotations;

namespace Employee.UI.ViewModels.HomePageViewModel
{
    public class HomeEmployeeDetailsViewModel
    {
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string EmpAddress { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfJoining { get; set; }
        public string EmpAadhar { get; set; }
        public string EmpPhoneNumber { get; set; }
        public string EmpEmail { get; set; }
        public string ImageURL { get; set; }
        public string DeptartmentName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
    }
}
