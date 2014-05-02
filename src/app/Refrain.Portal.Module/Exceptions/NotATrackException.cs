namespace Refrain.Portal.Module.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class NotATrackException : Exception
    {
        public NotATrackException()
        {
        }

        public NotATrackException(string message) : base(message)
        {
        }

        public NotATrackException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotATrackException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}