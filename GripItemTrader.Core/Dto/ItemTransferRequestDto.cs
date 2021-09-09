using GripItemTrader.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripItemTrader.Core.Dto
{
    public class ItemTransferRequestDto
    {
        /// <summary>
        /// Person that will receive the item
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Item to be transfered
        /// </summary>
        public int ItemId { get; set; }
    }
}
