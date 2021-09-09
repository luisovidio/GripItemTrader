using GripItemTrader.Core.Dto;
using GripItemTrader.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripItemTrader.Core.Interfaces.UseCases
{
    public interface IItemUseCases
    {
        Task<IEnumerable<Item>> GetAllItemsAsync();
        Task<Item> GetItemByIdAsync(int id);
        Task InsertItemAsync(ItemCreateRequestDto itemCreateRequest);
        Task UpdateItemAsync(ItemUpdateRequestDto itemUpdateRequest);
        Task DeleteItemAsync(int itemId);
    }
}
