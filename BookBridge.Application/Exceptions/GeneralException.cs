using System.Runtime.Serialization;

namespace BookBridge.Application.Exceptions
{
    public class GeneralException : Exception
    {
        public GeneralException()
        {
        }

        public GeneralException(string? message) : base(message)
        {
        }

        public GeneralException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected GeneralException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
