using GripItemTrader.Core.Dto;
using GripItemTrader.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripItemTrader.Core.Interfaces.UseCases
{
    public interface IItemTransferUseCases
    {
        Task<IEnumerable<ItemTransfer>> GetAllItemTransfersAsync();
        Task TransferItemAsync(ItemTransferRequestDto itemTransferDto);
    }
}
