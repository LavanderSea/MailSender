using MailSenderAPI;
using MailSenderClient;
using MailSenderClient.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Net.Http;

namespace IntegrationTests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void RegisterMailSenderService(IServiceCollection services)
        {
            services.AddSingleton(services.AddSingleton<MailSenderService>(provider =>
                new TestMailSenderService(provider.GetService<IRepository<Mail>>(), provider.GetService<ISender>())));
        }
    }

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
                .UseStartup<TestStartup>());

            Client = Server.CreateClient();
        }

        protected string GetConnectionString()
        {
            return Configuration.GetSection("DbConfiguration").GetValue<string>("ConnectionString");
        }
    }
}