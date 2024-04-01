using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeEntity
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int StateId { get; set; }

        [ForeignKey("StateId")]
        public State State { get; set; }
        //public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
