using GripItemTrader.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GripItemTrader.Core.Exceptions
{
    [Serializable()]
    public class GripItemTraderException : Exception
    {
        public GripItemTraderError ErrorCode { get; private set; }

        public GripItemTraderException(GripItemTraderError errorCode) : base()
        {
            ErrorCode = errorCode;
        }

        public GripItemTraderException(GripItemTraderError errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        protected GripItemTraderException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
