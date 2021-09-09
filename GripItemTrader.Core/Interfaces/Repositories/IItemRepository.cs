using GripItemTrader.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripItemTrader.Core.Interfaces.Repositories
{
    public interface IItemRepository : IBaseRepository<Item>
    {
        Task<bool> IsUniqueAsync(Item item);
        Task<bool> IsPersonIdValidAsync(int personId);
    }
}
