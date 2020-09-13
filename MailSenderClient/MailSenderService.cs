using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MailSenderClient.Exceptions;
using MailSenderClient.Infrastructure;
using MailSenderClient.Models;

namespace MailSenderClient
{
    public class MailSenderService
    {
        private readonly IRepository<Message> _repository;
        private readonly ISender _sender;

        public MailSenderService(IRepository<Message> repository, ISender sender)
        {
            _repository = repository;
            _sender = sender;
        }

        /// <summary>
        ///     Method that sends a message with given subject and body using email addresses from list of recipients. Then it logs
        ///     the result to the database.
        /// </summary>
        public void SendMessage(string subject, string body, IEnumerable<string> recipients)
        {
            CheckForNull(subject, body, recipients);

            if (!recipients.Any())
                throw new IncorrectFieldException("Empty list of recipients");

            if (recipients.Any(recipient => string.IsNullOrEmpty(recipient) || !IsValidEmail(recipient)))
                throw new IncorrectFieldException("Found incorrect email");

            var response = _sender.Send(subject, body, recipients);

            _repository.Insert(new Message(subject, body, recipients, GetActualTime(), response));
        }

        private void CheckForNull(params object[] objects)
        {
            if (objects.Any(obj => obj == null))
                throw new IncorrectFieldException("Fewer fields than necessary");
        }

        private bool IsValidEmail(string email)
        {
            const string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            var isMatch = Regex.Match(email, pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }

        /// <summary>
        ///     Method that receives all messages from database
        /// </summary>
        public IEnumerable<Message> GetAllMessages() => _repository.GetAll();

        protected virtual DateTimeOffset GetActualTime() => DateTimeOffset.Now;
    }
}