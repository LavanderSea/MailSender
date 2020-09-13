using System;
using MailSenderClient;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Resource = IntegrationTests.MailSenderAPITestsResource;

namespace IntegrationTests
{
    public class MailSenderAPITests : DatabaseSetUp
    {
        [Test]
        public async Task GetAllMailsWithAddresses_GetRequest_CorrectResponse()
        {
            var response = await Client.GetAsync("/api/mails");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(Resource.CorrectMails, responseString);
        }

        [Test]
        public async Task GetAllMailsWithAddressesWhenTablesAreEmpty_GetRequest_CorrectResponse()
        {
            DeleteAllData();
            var response = await Client.GetAsync("/api/mails");
            var result = response.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("Mails did not found", result);
        }

        [Test]
        public async Task PostMail_CorrectMail_Ok()
        {
            var httpContent =
                new StringContent(Resource.CorrectMail, Encoding.UTF8, "application/json");
            var expectedMail = new Mail(
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
            var actualMail = GetLastMail();

            Assert.AreEqual(expectedMail, actualMail);
        }

        [Test]
        public async Task PostMail_MailWithNullSubject_BadRequest()
        {
            var httpContent =
                new StringContent(Resource.IncorrectMail_NullSubject, Encoding.UTF8, "application/json");
            
            var response =
                await Client.PostAsync("/api/mails", httpContent);

            var result = response.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("Fewer fields than necessary", result);
        }

        [Test]
        public async Task PostMail_MailWithNullBody_BadRequest()
        {
            var httpContent =
                new StringContent(Resource.IncorrectMail_NullBody, Encoding.UTF8, "application/json");

            var response =
                await Client.PostAsync("/api/mails", httpContent);

            var result = response.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("Fewer fields than necessary", result);
        }

        [Test]
        public async Task PostMail_MailWithNullRecipients_BadRequest()
        {
            var httpContent =
                new StringContent(Resource.IncorrectMail_NullRecipients, Encoding.UTF8, "application/json");

            var response =
                await Client.PostAsync("/api/mails", httpContent);

            var result = response.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("Fewer fields than necessary", result);
        }

        [Test]
        public async Task PostMail_MailWithIncorrectEmailAddress_BadRequest()
        {
            var httpContent =
                new StringContent(Resource.MailWithIncorrectEmailAddress, Encoding.UTF8, "application/json");

            var response =
                await Client.PostAsync("/api/mails", httpContent);

            var result = response.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("Founded incorrect email", result);
        }

        [Test]
        public async Task PostMail_MailWithEmptyRecipients_BadRequest()
        {
            var httpContent =
                new StringContent(Resource.IncorrectMail_EmptyRecipients, Encoding.UTF8, "application/json");

            var response =
                await Client.PostAsync("/api/mails", httpContent);

            var result = response.Content.ReadAsStringAsync().Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual("Empty list of emails", result);
        }
    }
}