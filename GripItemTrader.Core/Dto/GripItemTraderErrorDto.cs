using GripItemTrader.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripItemTrader.Core.Dto
{
    public class GripItemTraderErrorDto
    {
        public static GripItemTraderErrorDto BuildErrorResponse(GripItemTraderException ex)
        {
            return new GripItemTraderErrorDto
            {
                ErrorCode = (int)ex.ErrorCode,
                ErrorCodeDescription = ex.ErrorCode.ToString(),
            };
        }

        public int ErrorCode { get; set; }

        public string ErrorCodeDescription { get; set; }
    }
}
