using System;
using MailSenderClient.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MailSenderClient.Exceptions;

namespace MailSenderClient
{
    public class MailSenderService
    {
        public MailSenderService(IRepository<Mail> repository, ISender sender)
        {
            _repository = repository;
            _sender = sender;
        }

        protected virtual DateTimeOffset GetActualTime()
        {
            return DateTimeOffset.Now;
        }

        public void SendMail(string subject, string body, IEnumerable<string> recipients)
        {
            CheckForNull(subject, body, recipients);

            if(!recipients.Any())
                throw new IncorrectFieldException("Empty list of emails");

            if (recipients.Any(recipient => string.IsNullOrEmpty(recipient) || !IsValidEmail(recipient)))
                throw new IncorrectFieldException("Founded incorrect email");

            var response = _sender.Send(subject, body, recipients);
            _repository.Set(new Mail(subject, body, recipients, GetActualTime(), response));
        }

        private void CheckForNull(params object[] objects)
        {
            foreach (var obj in objects)
            {
                if (obj == null)
                    throw new IncorrectFieldException("Fewer fields than necessary");
            }
        }

        private bool IsValidEmail(string email)
        {
            var pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            var isMatch = Regex.Match(email, pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }

        public IEnumerable<Mail> GetAllMails()
        {
            return _repository.GetAll();
        }

        private readonly IRepository<Mail> _repository;
        private readonly ISender _sender;

    }
}