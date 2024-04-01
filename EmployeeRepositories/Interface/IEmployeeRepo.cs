using EmployeeEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRepositories.Interface
{
    public interface IEmployeeRepo
    {
        Task<IEnumerable<Employee>> GetAll();
        Task<Employee> GetById(int id);
        Task Save(Employee employee);
        Task Edit(Employee employee);
        Task RemoveData(Employee employee);
    }
}
