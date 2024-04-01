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
    public class CityRepo : ICityRepo
    {
        private readonly ApplicationDbContext _context;

        public CityRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Edit(City city)
        {
            _context.Cities.Update(city);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<City>> GetAll()
        {
            return await _context.Cities.Include(c=>c.State).ToListAsync();
        }

        public async Task<City> GetById(int id)
        {
            return await _context.Cities.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task RemoveData(City city)
        {
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
        }

        public async Task Save(City city)
        {
            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();
        }
    }
}
