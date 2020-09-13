using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using MailSenderClient.Models;

namespace MailSenderAPI
{
    public static class Extensions
    {
        /// <summary>
        ///     Serialize messages to json
        /// </summary>
        public static string ToJson(this IEnumerable<Message> messages)
        {
            return JsonSerializer.Serialize(messages.Select(message => new
            {
                message.Subject,
                message.Body,
                message.Recipients,
                date = message.Date.ToString("g"),
                message.Response.Result,
                message.Response.FailedMessage
            }));
        }
    }
}