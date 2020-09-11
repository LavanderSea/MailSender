using System;
using System.Runtime.Serialization;

namespace MailSenderClient.Exceptions
{
    [Serializable]
    public class IncorrectFieldException : Exception
    {
        
        public IncorrectFieldException() {}

        public IncorrectFieldException(string message) : base(message) {}

        public IncorrectFieldException(string message, Exception inner) : base(message, inner) {}

        protected IncorrectFieldException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) {}
    }
}