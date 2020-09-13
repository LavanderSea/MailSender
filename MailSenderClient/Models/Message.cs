using System;
using System.Collections.Generic;
using System.Linq;

namespace MailSenderClient.Models
{
    public class Message
    {
        public Message(string subject, string body, IEnumerable<string> recipients, DateTimeOffset date,
            Response response)
        {
            Subject = subject;
            Body = body;
            Recipients = recipients;
            Date = date;
            Response = response;
        }

        public string Subject { get; }
        public string Body { get; }
        public IEnumerable<string> Recipients { get; }
        public DateTimeOffset Date { get; }

        /// <summary>
        ///     Result of sending a message
        /// </summary>
        public Response Response { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Message) obj);
        }

        protected bool Equals(Message other)
        {
            return Body == other.Body &&
                   Recipients.SequenceEqual(other.Recipients) &&
                   Equals(Response, other.Response) &&
                   Subject == other.Subject &&
                   Date.Equals(other.Date);
        }

        public override int GetHashCode() => HashCode.Combine(Body, Recipients, Response, Subject, Date);
        public static bool operator ==(Message left, Message right) => Equals(left, right);
        public static bool operator !=(Message left, Message right) => !Equals(left, right);
    }
}