using GripItemTrader.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripItemTrader.Core.Interfaces.Repositories
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        Task<bool> IsUniqueAsync(Person person);
    }
}
