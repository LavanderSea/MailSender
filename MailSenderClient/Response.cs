using System;

namespace MailSenderClient
{
    public class Response
    {
        public Response(string result, string failedMessage)
        {
            Result = result;
            FailedMessage = failedMessage;
        }

        protected bool Equals(Response other)
        {
            return FailedMessage == other.FailedMessage && Result == other.Result;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Response) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FailedMessage, Result);
        }

        public static bool operator ==(Response left, Response right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Response left, Response right)
        {
            return !Equals(left, right);
        }

        public string Result { get; }
        public string FailedMessage { get; }
    }
}