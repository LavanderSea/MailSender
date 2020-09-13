using System;
using System.Collections.Generic;
using MailSenderClient;
using MailSenderClient.Infrastructure;

namespace MailSender.Tests.Stubs
{
    public class StubRepository : IRepository<Mail>
    {
        public Mail Mail { get; private set; }

        public IEnumerable<Mail> Messages { get; } = new[]
        {
            new Mail("subject", "body", new[] {"mail@mail.ru", "mail@gmail.com"}, DateTimeOffset.MinValue, 
                new Response("Ok", string.Empty)),
            new Mail("subject_2", "body_2", new[] {"mail2@mail.ru", "mail2@gmail.com"}, DateTimeOffset.MinValue,
                new Response("Failed", "Just because"))
        };

        public void Set(Mail mail)
        {
            Mail = mail;
        }

        public IEnumerable<Mail> GetAll()
        {
            return Messages;
        }
    }
}