using System;
using MailSenderClient;
using MailSenderClient.Infrastructure;

namespace IntegrationTests
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
