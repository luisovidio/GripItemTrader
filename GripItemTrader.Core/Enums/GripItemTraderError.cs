using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripItemTrader.Core.Enums
{
    public enum GripItemTraderError
    {
        PERSON_NOT_FOUND,
        PERSON_NOT_UNIQUE,
        PERSON_NAME_REQUIRED,
        ITEM_NOT_FOUND,
        ITEM_NOT_UNIQUE,
        ITEM_NAME_REQUIRED,
        INVALID_TRANSFER,
        ITEM_NAME_TOO_LONG,
        PERSON_NAME_TOO_LONG
    }
}
