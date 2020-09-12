using System.Net.Http;
using MailSenderAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace IntegrationTests
{
    public abstract class WebApplicationLifeCycle
    {
        protected HttpClient Client;
        protected IConfigurationRoot Configuration;
        protected TestServer Server;

        [OneTimeTearDown]
        protected void Tear()
        {
            Server.Dispose();
            Client.Dispose();
        }

        [OneTimeSetUp]
        protected void SetUp()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();

            Server = new TestServer(new WebHostBuilder()
                .UseConfiguration(Configuration)
                .UseEnvironment("test")
                .UseStartup<Startup>());

            Client = Server.CreateClient();
        }

        protected string GetConnectionString()
        {
            return Configuration.GetSection("DbConfiguration").GetValue<string>("ConnectionString");
        }
    }
}