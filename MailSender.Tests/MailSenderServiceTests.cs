using MailSender.Tests.Stubs;
using MailSenderClient;
using MailSenderClient.Exceptions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MailSender.Tests
{
    public class MailSenderServiceTests
    {
        [SetUp]
        public void Setup()
        {
            var sender = CreateSenderMock();
            var repository = new StubRepository();
            _service = new TestMailSenderService(repository, sender);
        }

        [Test]
        public void GetMails_CorrectMails()
        {
            var sender = CreateSenderMock();
            var repository = new StubRepository();
            var service = new TestMailSenderService(repository, sender);

            var messages = service.GetAllMails();

            Assert.AreEqual(repository.Messages, messages);
        }

        [Test]
        public void SendMail_CorrectMail_Ok()
        {
            var message = new Mail("subject", "body", new[] { "mail@mail.ru", "mail@gmail.com" }, DateTimeOffset.MinValue,
                new Response("Ok", string.Empty));

            var sender = CreateSenderMock();
            var repository = new StubRepository();
            var service = new TestMailSenderService(repository, sender);

            service.SendMail("subject", "body", new[] { "mail@mail.ru", "mail@gmail.com" });

            Assert.AreEqual(message, repository.Mail);
        }

        [Test]
        public void SendMail_NullSubject_IncorrectFieldException()
        {
            void Sending()
            {
                _service.SendMail(null, "body", new[] { "mail@mail.ru", "mail@gmail.com" });
            }

            var exception = Assert.Throws<IncorrectFieldException>(Sending);
            Assert.AreEqual("Fewer fields than necessary", exception.Message);
        }

        [Test]
        public void SendMail_NullBody_IncorrectFieldException()
        {
            void Sending()
            {
                _service.SendMail("subject", null, new[] { "mail@mail.ru", "mail@gmail.com" });
            }

            var exception = Assert.Throws<IncorrectFieldException>(Sending);
            Assert.AreEqual("Fewer fields than necessary", exception.Message);
        }

        [Test]
        public void SendMail_IncorrectEmail_IncorrectFieldException()
        {
            void Sending()
            {
                _service.SendMail("subject", "body", new[] { "mailmail.ru", "mail@gmail.com" });
            }

            var exception = Assert.Throws<IncorrectFieldException>(Sending);
            Assert.AreEqual("Founded incorrect email", exception.Message);
        }

        [Test]
        public void SendMail_NullEmail_IncorrectFieldException()
        {
            void Sending()
            {
                _service.SendMail("subject", "body", new[] { null, "mail@gmail.com" });
            }

            var exception = Assert.Throws<IncorrectFieldException>(Sending);
            Assert.AreEqual("Founded incorrect email", exception.Message);
        }

        [Test]
        public void SendMail_EmptyEmail_IncorrectFieldException()
        {
            void Sending()
            {
                _service.SendMail("subject", "body", new[] { string.Empty, "mail@gmail.com" });
            }

            var exception = Assert.Throws<IncorrectFieldException>(Sending);
            Assert.AreEqual("Founded incorrect email", exception.Message);
        }

        [Test]
        public void SendMail_NullEmails_IncorrectFieldException()
        {
            void Sending()
            {
                _service.SendMail("subject", "body", null);
            }

            var exception = Assert.Throws<IncorrectFieldException>(Sending);
            Assert.AreEqual("Fewer fields than necessary", exception.Message);
        }

        [Test]
        public void SendMail_EmptyEmails_IncorrectFieldException()
        {
            void Sending()
            {
                _service.SendMail("subject", "body", Array.Empty<string>());
            }

            var exception = Assert.Throws<IncorrectFieldException>(Sending);
            Assert.AreEqual("Empty list of emails", exception.Message);
        }

        public ISender CreateSenderMock()
        {
            var mock = new Mock<ISender>();
            mock.Setup(sender =>
                    sender.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns(new Response("Ok", string.Empty));
            return mock.Object;
        }

        private MailSenderService _service;
    }


}