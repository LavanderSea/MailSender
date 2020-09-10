using System.Collections.Generic;

namespace MailSenderClient
{
    public class Message
    {
        public Message(string subject, string body, IEnumerable<string> recipients, Response response)
        {
            Subject = subject;
            Body = body;
            Recipients = recipients;
            Response = response;
        }

        public string Subject { get; }
        public string Body { get; }
        public IEnumerable<string> Recipients { get; }
        public Response Response { get; }
    }
}