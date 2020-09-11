using MailSenderClient;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MailSenderAPI
{
    public static class Extensions
    {
        public static string ToJson(this IEnumerable<Mail> mails)
        {
            return JsonSerializer.Serialize(mails.Select(mail => new
            {
                mail.Subject,
                mail.Body,
                mail.Recipients,
                mail.Response.Result,
                mail.Response.FailedMessage
            }));
        }
    }
}