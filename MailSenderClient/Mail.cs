using System;
using System.Collections.Generic;
using System.Linq;

namespace MailSenderClient
{
    public class Mail
    {
        public Mail(string subject, string body, IEnumerable<string> recipients, DateTimeOffset date, Response response)
        {
            Subject = subject;
            Body = body;
            Recipients = recipients;
            Date = date;
            Response = response;
        }

        public DateTimeOffset Date { get; }
        public string Subject { get; }
        public string Body { get; }
        public IEnumerable<string> Recipients { get; }
        public Response Response { get; }

        protected bool Equals(Mail other)
        {
            return Body == other.Body &&
                   Recipients.SequenceEqual(other.Recipients) &&
                   Equals(Response, other.Response) &&
                   Subject == other.Subject &&
                   Date.Equals(other.Date);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((Mail) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Body, Recipients, Response, Subject, Date);
        }

        public static bool operator ==(Mail left, Mail right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Mail left, Mail right)
        {
            return !Equals(left, right);
        }
    }
}