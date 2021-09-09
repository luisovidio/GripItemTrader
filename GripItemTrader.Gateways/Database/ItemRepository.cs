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
    public class ItemRepository : BaseRepository<Item>, IItemRepository
    {
        public ItemRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> IsPersonIdValidAsync(int personId)
        {
            return await _context.Person.AnyAsync(p => p.Id == personId);
        }

        public async Task<bool> IsUniqueAsync(Item item)
        {
            var query = (item.Id > 0)
                ? _context.Item.Where(i => i.Id != item.Id)
                : _context.Item;

            return await query.AllAsync(i => i.Name != item.Name);
        }
    }
}
