using MailSenderClient.Infrastructure;
using System.Collections.Generic;

namespace MailSenderClient
{
    public class MailSenderService
    {
        public MailSenderService(IRepository<Message> repository, ISender sender)
        {
            _repository = repository;
            _sender = sender;
        }

        public void SendMail(string subject, string body, IEnumerable<string> recipients)
        {
            var response = _sender.Send(subject, body, recipients);
            _repository.Set(new Message(subject, body, recipients, response));
        }

        public IEnumerable<Message> GetAllMails()
        {
            return _repository.GetAll();
        }

        private readonly IRepository<Message> _repository;
        private readonly ISender _sender;

    }
}