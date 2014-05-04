namespace Refrain.Portal.Module.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class CannotMapException : Exception
    {
        public CannotMapException()
        {
        }

        public CannotMapException(string message) : base(message)
        {
        }

        public CannotMapException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CannotMapException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}