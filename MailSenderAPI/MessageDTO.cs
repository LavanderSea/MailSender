using System.Collections.Generic;

namespace MailSenderAPI
{
    public class MessageDTO
    {
        public MessageDTO(string subject, string body, IEnumerable<string> recipients)
        {
            Subject = subject;
            Body = body;
            Recipients = recipients;
        }

        public string Subject { get; }
        public string Body { get; }
        public IEnumerable<string> Recipients { get; }
    }
}