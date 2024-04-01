using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeEntity
{
    public class Employee
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfJoining { get; set; }
        public string Aadhar { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImageURL { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public int StateId { get; set; }
        public State State { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
    }
}
