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
    public class StateRepo : IStateRepo
    {
        private readonly ApplicationDbContext _context;

        public StateRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Edit(State state)
        {
            _context.States.Update(state);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<State>> GetAll()
        {
            return await _context.States.ToListAsync();
        }

        public async Task<State> GetById(int id)
        {
            return await _context.States.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task RemoveData(State state)
        {
            _context.States.Remove(state);
            await _context.SaveChangesAsync();
        }

        public async Task Save(State state)
        {
            await _context.States.AddAsync(state);
            await _context.SaveChangesAsync();
        }
    }
}
