using GripItemTrader.Core.Entities;
using GripItemTrader.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripItemTrader.Gateways.Database
{
    public class ItemTransferRepository : BaseRepository<ItemTransfer>, IItemTransferRepository
    {
        public ItemTransferRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Item> GetItemByIdAsync(int itemId)
        {
            return await _context.Item.SingleOrDefaultAsync(i => i.Id == itemId);
        }

        public async Task<bool> IsPersonIdValidAsync(int personId)
        {
            return await _context.Person.AnyAsync(p => p.Id == personId);
        }
    }
}
