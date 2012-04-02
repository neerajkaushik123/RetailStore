using System;
using System.Runtime.Serialization;

namespace Retail.Models
{
    [Serializable]
    public class ValidationException : ApplicationException
    {
        public ValidationException() { }
        public ValidationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public ValidationException(string msg)
            : base(msg)
        { }
        public ValidationException(string msg,Exception ex)
            : base(msg,ex)
        { }

    }
}
