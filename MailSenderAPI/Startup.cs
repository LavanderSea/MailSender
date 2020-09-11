using MailSenderClient;
using MailSenderClient.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace MailSenderAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var stmpConfiguration = Configuration.GetSection("STMPConfiguration");
            var password = GetConfigurationValue("Password", stmpConfiguration);
            var userId = GetConfigurationValue("UserId", stmpConfiguration);
            services.AddSingleton<ISender>(provider => new GmailSender(password, userId));

            var dbConfiguration = Configuration.GetSection("DbConfiguration");
            var connectionString = GetConfigurationValue("ConnectionString", dbConfiguration);
            services.AddSingleton<IRepository<Mail>>(provider => new SqlRepository(connectionString));

            services.AddSingleton<MailSenderService>(provider =>
                new MailSenderService(provider.GetService<IRepository<Mail>>(), provider.GetService<ISender>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static string GetConfigurationValue(string sectionKey, IConfigurationSection configurationSection)
        {
            var configurationValue = configurationSection[sectionKey];
            if (string.IsNullOrEmpty(configurationValue))
                throw new ArgumentException($"{sectionKey} operation name is null or empty");

            return configurationValue;
        }
    }
}