﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripItemTrader.Core.Entities
{
    public class ItemTransfer : BaseEntity
    {
        public int FromPersonId { get; set; }
        public int ToPersonId { get; set; }
        public int ItemId { get; set; }
    }
}
