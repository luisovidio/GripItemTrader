using GripItemTrader.Core.Dto;
using GripItemTrader.Core.Entities;
using GripItemTrader.Core.Exceptions;
using GripItemTrader.Core.Interfaces;
using GripItemTrader.Core.Interfaces.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GripItemTrader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemUseCases _itemUseCases;

        public ItemController(IItemUseCases itemUseCases)
        {
            _itemUseCases = itemUseCases;
        }

        /// <summary>
        /// Get all items
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllItemsAsync()
        {
            var result = await _itemUseCases.GetAllItemsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get an item by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemByIdAsync(int id)
        {
            try
            {
                var result = await _itemUseCases.GetItemByIdAsync(id);
                return Ok(result);
            }
            catch(GripItemTraderException ex)
            {
                return BadRequest(GripItemTraderErrorDto.BuildErrorResponse(ex));
            }
            
        }

        /// <summary>
        /// Insert a new item
        /// </summary>
        /// <param name="item">Item to be inserted</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostItemAsync(ItemCreateRequestDto itemCreateRequest)
        {
            try
            {
                await _itemUseCases.InsertItemAsync(itemCreateRequest);
                return Ok();
            }
            catch (GripItemTraderException ex)
            {
                return BadRequest(GripItemTraderErrorDto.BuildErrorResponse(ex));
            }
        }

        /// <summary>
        /// Update the given item
        /// </summary>
        /// <param name="item">Person to be updated</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> PutItemAsync(ItemUpdateRequestDto itemUpdateRequest)
        {
            try
            {
                await _itemUseCases.UpdateItemAsync(itemUpdateRequest);
                return Ok();
            }
            catch (GripItemTraderException ex)
            {
                return BadRequest(GripItemTraderErrorDto.BuildErrorResponse(ex));
            }
        }

        /// <summary>
        /// Soft delete an item
        /// </summary>
        /// <param name="personId">Id from the item to be deleted</param>
        /// <returns></returns>
        [HttpDelete("itemId")]
        public async Task<IActionResult> DeleteItemAsync(int itemId)
        {
            try
            {
                await _itemUseCases.DeleteItemAsync(itemId);
                return Ok();
            }
            catch (GripItemTraderException ex)
            {
                return BadRequest(GripItemTraderErrorDto.BuildErrorResponse(ex));
            }
        }
    }
}
