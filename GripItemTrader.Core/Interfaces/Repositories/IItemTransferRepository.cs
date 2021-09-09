using GripItemTrader.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripItemTrader.Core.Interfaces.Repositories
{
    public interface IItemTransferRepository : IBaseRepository<ItemTransfer>
    {
        Task<bool> IsPersonIdValidAsync(int personId);
        Task<Item> GetItemByIdAsync(int itemId);
    }
}
