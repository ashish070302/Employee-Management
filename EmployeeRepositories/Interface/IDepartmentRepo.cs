using EmployeeEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRepositories.Interface
{
    public interface IDepartmentRepo
    {
        Task<IEnumerable<Department>> GetAll();
        Task<Department> GetById(int id);
        Task Save(Department department);
        Task Edit(Department department);
        Task RemoveData(Department department);
    }
}
