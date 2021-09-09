using GripItemTrader.Core.Dto;
using GripItemTrader.Core.Entities;
using GripItemTrader.Core.Enums;
using GripItemTrader.Core.Exceptions;
using GripItemTrader.Core.Interfaces.Repositories;
using GripItemTrader.Core.Interfaces.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripItemTrader.UseCases
{
    public class ItemTransferUseCases : IItemTransferUseCases
    {
        private readonly IItemTransferRepository _itemTransferRepository;

        public ItemTransferUseCases(IItemTransferRepository itemTransferRepository)
        {
            _itemTransferRepository = itemTransferRepository;
        }

        public async Task<IEnumerable<ItemTransfer>> GetAllItemTransfersAsync()
        {
            return await _itemTransferRepository.GetAllAsync();
        }

        public async Task TransferItemAsync(ItemTransferRequestDto itemTransferDto)
        {
            await EnsurePersonExists(itemTransferDto.PersonId);
            var item = await EnsureItemExists(itemTransferDto.ItemId);

            var itemTransfer = ItemTransferFactory(item, itemTransferDto.PersonId);

            EnsureValidTransfer(itemTransfer);

            item.PersonId = itemTransferDto.PersonId;
            await _itemTransferRepository.InsertAsync(itemTransfer);

            await _itemTransferRepository.SaveChangesAsync();
        }

        private static void EnsureValidTransfer(ItemTransfer itemTransfer)
        {
            if (itemTransfer.FromPersonId == itemTransfer.ToPersonId)
                throw new GripItemTraderException(GripItemTraderError.INVALID_TRANSFER);
        }

        private async Task EnsurePersonExists(int personId)
        {
            bool personExists = await _itemTransferRepository.IsPersonIdValidAsync(personId);
            if (!personExists)
                throw new GripItemTraderException(GripItemTraderError.PERSON_NOT_FOUND);
        }

        private async Task<Item> EnsureItemExists(int itemId)
        {
            var item = await _itemTransferRepository.GetItemByIdAsync(itemId);
            if (item != null)
                return item;

            throw new GripItemTraderException(GripItemTraderError.ITEM_NOT_FOUND);
        }

        private static ItemTransfer ItemTransferFactory(Item item, int personId)
        {
            return new ItemTransfer
            {
                FromPersonId = item.PersonId,
                ToPersonId = personId,
                ItemId = item.Id
            };
        }
    }
}
