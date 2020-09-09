using System.Collections.Generic;

namespace MailSenderClient
{
    public interface ISender
    {
        Response Send(string subject, string body, IEnumerable<string> recipients);
    }
}