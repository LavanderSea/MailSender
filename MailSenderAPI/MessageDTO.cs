using System.Collections.Generic;

namespace MailSenderAPI
{
    public class MessageDto
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public IEnumerable<string> Recipients { get; set; }
    }
}