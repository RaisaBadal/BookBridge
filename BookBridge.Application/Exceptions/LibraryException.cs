using System.Runtime.Serialization;

namespace BookBridge.Application.Exceptions
{
    public class LibraryException : Exception
    {
        public LibraryException()
        {
        }

        public LibraryException(string? message) : base(message)
        {
        }

        public LibraryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected LibraryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
