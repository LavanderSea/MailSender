using System;
using System.Collections.Generic;
using MailSenderClient.Infrastructure;
using MailSenderClient.Models;

namespace MailSender.Tests.Stubs
{
    public class StubRepository : IRepository<Message>
    {
        public Message Message { get; private set; }

        public IEnumerable<Message> Messages { get; } = new []
        {
            new Message(
                "subject",
                "body",
                new[]
                {
                    "mail@mail.ru",
                    "mail@gmail.com"
                },
                DateTimeOffset.MinValue,
                new Response("Ok", string.Empty)),
            new Message(
                "subject_2",
                "body_2",
                new[]
                {
                    "mail2@mail.ru",
                    "mail2@gmail.com"
                },
                DateTimeOffset.MinValue,
                new Response("Failed", "Just because"))
        };

        public void Insert(Message message) => Message = message;
        public IEnumerable<Message> GetAll() => Messages;
    }
}