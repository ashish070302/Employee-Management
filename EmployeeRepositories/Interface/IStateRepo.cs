﻿using EmployeeEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeRepositories.Interface
{
    public interface IStateRepo
    {
        Task<IEnumerable<State>> GetAll();
        Task<State> GetById(int id);
        Task Save(State state);
        Task Edit(State state);
        Task RemoveData(State state);
    }
}
