using GripItemTrader.Core.Dto;
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
    public class ItemTransferController : ControllerBase
    {
        private readonly IItemTransferUseCases _itemTransferUseCases;

        public ItemTransferController(IItemTransferUseCases itemTransferUseCases)
        {
            _itemTransferUseCases = itemTransferUseCases;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllItemTransfersAsync()
        {
            var result = await _itemTransferUseCases.GetAllItemTransfersAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostItemTransferAsync(ItemTransferRequestDto itemTransfer)
        {
            try
            {
                await _itemTransferUseCases.TransferItemAsync(itemTransfer);
                return Ok();
            }
            catch (GripItemTraderException ex)
            {
                return BadRequest(GripItemTraderErrorDto.BuildErrorResponse(ex));
            }
        }   
    }
}
