using System;
using MailSenderClient;
using MailSenderClient.Infrastructure;
using MailSenderClient.Models;

namespace MailSender.Tests.Stubs
{
    public class TestMailSenderService : MailSenderService
    {
        public TestMailSenderService(IRepository<Message> repository, ISender sender) : base(repository, sender)
        {
        }

        protected override DateTimeOffset GetActualTime() => DateTimeOffset.MinValue;
    }
}