using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MailSenderClient.Models;
using NUnit.Framework;
using Resource = IntegrationTests.MailSenderAPITestsResource;

namespace IntegrationTests
{
    public class MailSenderAPITests : DatabaseSetUp
    {
        [Test]
        public async Task GetAllMessagesWithAddresses_GetRequest_CorrectResponse()
        {
            DeleteAllData();
            InsertData();

            var response = await Client.GetAsync("/api/mails");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(Resource.CorrectMessages, responseString);
        }

        [Test]
        public async Task GetAllMessagesWhenTablesAreEmpty_GetRequest_CorrectResponse()
        {
            DeleteAllData();
            var response = await Client.GetAsync("/api/mails");
            var result = response.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("Messages did not found", result);
        }

        [Test]
        public async Task PostMessage_CorrectMessage_Ok()
        {
            var httpContent =
                new StringContent(Resource.CorrectMessage, Encoding.UTF8, "application/json");
            var expectedMessage = new Message(
                "Hello",
                "Body here",
                new[]
                {
                    "ta.nya.smith1712@gmail.com",
                    "tanya.smith1712@gmail.com"
                },
                DateTimeOffset.MinValue,
                new Response(
                    "Ok",
                    string.Empty));

            var response =
                await Client.PostAsync("/api/mails", httpContent);
            response.EnsureSuccessStatusCode();
            var actualMessage = GetLastMessage();

            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [Test]
        public async Task PostMessage_MessageWithNullSubject_BadRequest()
        {
            var httpContent =
                new StringContent(Resource.IncorrectMessage_NullSubject, Encoding.UTF8, "application/json");

            var response =
                await Client.PostAsync("/api/mails", httpContent);

            var result = response.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("Fewer fields than necessary", result);
        }

        [Test]
        public async Task PostMessage_MessageWithNullBody_BadRequest()
        {
            var httpContent =
                new StringContent(Resource.IncorrectMessage_NullBody, Encoding.UTF8, "application/json");

            var response =
                await Client.PostAsync("/api/mails", httpContent);

            var result = response.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("Fewer fields than necessary", result);
        }

        [Test]
        public async Task PostMessage_MessageWithNullRecipients_BadRequest()
        {
            var httpContent =
                new StringContent(Resource.IncorrectMessage_NullRecipients, Encoding.UTF8, "application/json");

            var response =
                await Client.PostAsync("/api/mails", httpContent);

            var result = response.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("Fewer fields than necessary", result);
        }

        [Test]
        public async Task PostMessage_MessageWithIncorrectEmailAddress_BadRequest()
        {
            var httpContent =
                new StringContent(Resource.MessageWithIncorrectEmailAddress, Encoding.UTF8, "application/json");

            var response =
                await Client.PostAsync("/api/mails", httpContent);

            var result = response.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("Found incorrect email", result);
        }

        [Test]
        public async Task PostMessage_MessageWithEmptyRecipients_BadRequest()
        {
            var httpContent =
                new StringContent(Resource.IncorrectMessage_EmptyRecipients, Encoding.UTF8, "application/json");

            var response =
                await Client.PostAsync("/api/mails", httpContent);

            var result = response.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("Empty list of recipients", result);
        }
    }
}