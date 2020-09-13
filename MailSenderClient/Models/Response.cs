using System;

namespace MailSenderClient.Models
{
    public class Response
    {
        public Response(string result, string failedMessage)
        {
            Result = result;
            FailedMessage = failedMessage;
        }

        /// <summary>
        ///     Status of sending a message. Can be "Ok" or "Failed".
        /// </summary>
        public string Result { get; }

        /// <summary>
        ///     Message about fail of sending.
        /// </summary>
        public string FailedMessage { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Response) obj);
        }

        protected bool Equals(Response other) => FailedMessage == other.FailedMessage && Result == other.Result;
        public override int GetHashCode() => HashCode.Combine(FailedMessage, Result);
        public static bool operator ==(Response left, Response right) => Equals(left, right);
        public static bool operator !=(Response left, Response right) => !Equals(left, right);
    }
}