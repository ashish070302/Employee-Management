using EmployeeEntity;
using EmployeeRepositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRepositories.Implementation
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Edit(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await _context.Employees.Include(s=>s.Department).Include(c=>c.State).Include(d => d.City).ToListAsync();
        }

        public async Task<Employee> GetById(int id)
        {
            return await _context.Employees.Include(a=>a.State).Include(b => b.City).Include(c => c.Department).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task RemoveData(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task Save(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }
    }
}
