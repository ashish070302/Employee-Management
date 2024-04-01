using EmployeeEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRepositories.Interface
    {
    public interface ICityRepo
    {
        Task<IEnumerable<City>> GetAll();
        Task<City> GetById(int id);
        Task Save(City city);
        Task Edit(City city);
        Task RemoveData(City city);
    }
}
