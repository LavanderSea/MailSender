using MailSenderClient;
using MailSenderClient.Infrastructure;
using System;

namespace MailSender.Tests.Stubs
{
    public class TestMailSenderService : MailSenderService
    {
        public TestMailSenderService(IRepository<Mail> repository, ISender sender) : base(repository, sender)
        {

        }

        protected override DateTimeOffset GetActualTime()
        {
            return DateTimeOffset.MinValue;
        }

    }
}
