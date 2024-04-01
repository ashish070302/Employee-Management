using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeEntity
{
    public class State
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        //public ICollection<City> Cities { get; set; }
    }
}
