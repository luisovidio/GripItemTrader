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
    public class ItemUseCases : IItemUseCases
    {
        private readonly IItemRepository _itemRepository;

        public ItemUseCases(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task DeleteItemAsync(int itemId)
        {
            var item = await EnsureExists(itemId);

            item.IsActive = false;

            await _itemRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            return await _itemRepository.GetAllAsync();
        }

        public async Task<Item> GetItemByIdAsync(int id)
        {
            var item = await EnsureExists(id);
            return item;
        }

        public async Task InsertItemAsync(ItemCreateRequestDto itemCreateRequest)
        {
            var item = ItemFactory(itemCreateRequest);

            EnsureItemIsValid(item);
            await EnsurePersonExists(item.PersonId);
            
            await EnsureIsUnique(item);

            await _itemRepository.InsertAsync(item);

            await _itemRepository.SaveChangesAsync();
        }

        public async Task UpdateItemAsync(ItemUpdateRequestDto itemUpdateRequest)
        {
            var currentItem = await EnsureExists(itemUpdateRequest.Id);
            currentItem.Name = itemUpdateRequest.Name.Trim();

            EnsureItemIsValid(currentItem);

            await EnsureIsUnique(currentItem);

            await _itemRepository.SaveChangesAsync();
        }

        private async Task<Item> EnsureExists(int itemId)
        {
            var item = await _itemRepository.GetByIdAsync(itemId);
            if (item != null)
                return item;
            throw new GripItemTraderException(GripItemTraderError.ITEM_NOT_FOUND);
        }

        private async Task EnsurePersonExists(int personId)
        {
            bool personExists = await _itemRepository.IsPersonIdValidAsync(personId);
            if (!personExists)
                throw new GripItemTraderException(GripItemTraderError.PERSON_NOT_FOUND);
        }

        private async Task EnsureIsUnique(Item item)
        {
            bool isUnique = await _itemRepository.IsUniqueAsync(item);
            if (!isUnique)
                throw new GripItemTraderException(GripItemTraderError.ITEM_NOT_UNIQUE);
        }

        private static void EnsureItemIsValid(Item item)
        {
            if (string.IsNullOrEmpty(item.Name))
                throw new GripItemTraderException(GripItemTraderError.ITEM_NAME_REQUIRED);
            if (item.Name.Length > 100)
                throw new GripItemTraderException(GripItemTraderError.ITEM_NAME_TOO_LONG);
        }

        private static Item ItemFactory(ItemCreateRequestDto itemCreateRequest)
        {
            return new Item
            {
                PersonId = itemCreateRequest.PersonId,
                Name = itemCreateRequest.Name.Trim(),
                IsActive = true
            };
        }
    }
}
