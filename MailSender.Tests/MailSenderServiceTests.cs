using System;
using System.Collections.Generic;
using MailSender.Tests.Stubs;
using MailSenderClient;
using MailSenderClient.Exceptions;
using MailSenderClient.Infrastructure;
using MailSenderClient.Models;
using Moq;
using NUnit.Framework;

namespace MailSender.Tests
{
    public class MailSenderServiceTests
    {
        private StubRepository _repository;
        private MailSenderService _service;

        [SetUp]
        public void Setup()
        {
            var sender = CreateSenderMock();
            _repository = new StubRepository();
            _service = new TestMailSenderService(_repository, sender);
        }

        [Test]
        public void GetMessages_CorrectMessages()
        {
            var messages = _service.GetAllMessages();

            Assert.AreEqual(_repository.Messages, messages);
        }

        [Test]
        public void SendMessage_CorrectMessage_Ok()
        {
            var message = new Message("subject", "body", new[] {"mail@mail.ru", "mail@gmail.com"},
                DateTimeOffset.MinValue,
                new Response("Ok", string.Empty));

            _service.SendMessage("subject", "body", new[] {"mail@mail.ru", "mail@gmail.com"});

            Assert.AreEqual(message, _repository.Message);
        }

        [Test]
        public void SendMessage_NullSubject_IncorrectFieldException()
        {
            void Sending()
            {
                _service.SendMessage(null, "body", new[] {"mail@mail.ru", "mail@gmail.com"});
            }

            var exception = Assert.Throws<IncorrectFieldException>(Sending);
            Assert.AreEqual("Fewer fields than necessary", exception.Message);
        }

        [Test]
        public void SendMessage_NullBody_IncorrectFieldException()
        {
            void Sending()
            {
                _service.SendMessage("subject", null, new[] {"mail@mail.ru", "mail@gmail.com"});
            }

            var exception = Assert.Throws<IncorrectFieldException>(Sending);
            Assert.AreEqual("Fewer fields than necessary", exception.Message);
        }

        [Test]
        public void SendMessage_IncorrectEmail_IncorrectFieldException()
        {
            void Sending()
            {
                _service.SendMessage("subject", "body", new[] {"mailmail.ru", "mail@gmail.com"});
            }

            var exception = Assert.Throws<IncorrectFieldException>(Sending);
            Assert.AreEqual("Found incorrect email", exception.Message);
        }

        [Test]
        public void SendMessage_NullEmail_IncorrectFieldException()
        {
            void Sending()
            {
                _service.SendMessage("subject", "body", new[] {null, "mail@gmail.com"});
            }

            var exception = Assert.Throws<IncorrectFieldException>(Sending);
            Assert.AreEqual("Found incorrect email", exception.Message);
        }

        [Test]
        public void SendMessage_EmptyEmail_IncorrectFieldException()
        {
            void Sending()
            {
                _service.SendMessage("subject", "body", new[] {string.Empty, "mail@gmail.com"});
            }

            var exception = Assert.Throws<IncorrectFieldException>(Sending);
            Assert.AreEqual("Found incorrect email", exception.Message);
        }

        [Test]
        public void SendMessage_NullRecipients_IncorrectFieldException()
        {
            void Sending()
            {
                _service.SendMessage("subject", "body", null);
            }

            var exception = Assert.Throws<IncorrectFieldException>(Sending);
            Assert.AreEqual("Fewer fields than necessary", exception.Message);
        }

        [Test]
        public void SendMessage_EmptyRecipients_IncorrectFieldException()
        {
            void Sending()
            {
                _service.SendMessage("subject", "body", Array.Empty<string>());
            }

            var exception = Assert.Throws<IncorrectFieldException>(Sending);
            Assert.AreEqual("Empty list of recipients", exception.Message);
        }

        public ISender CreateSenderMock()
        {
            var mock = new Mock<ISender>();
            mock.Setup(sender =>
                    sender.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                .Returns(new Response("Ok", string.Empty));
            return mock.Object;
        }
    }
}