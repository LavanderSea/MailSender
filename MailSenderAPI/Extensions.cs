using MailSenderClient;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MailSenderAPI
{
    public static class Extensions
    {
        public static string ToJson(this IEnumerable<Message> messages)
        {
            return JsonSerializer.Serialize(messages.Select(message => new
            {
                message.Subject,
                message.Body,
                message.Recipients,
                message.Response.Result,
                message.Response.FailedMessage
            }));
        }
    }
}