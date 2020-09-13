using System.Collections.Generic;
using MailSenderClient.Models;

namespace MailSenderClient.Infrastructure
{
    public interface ISender
    {
        /// <summary>
        ///     Method that sends a message with given subject and body using email addresses from list of recipients.
        /// </summary>
        Response Send(string subject, string body, IEnumerable<string> recipients);
    }
}