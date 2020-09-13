using System;
using System.Globalization;
using System.Linq;
using Dapper;
using MailSenderClient;
using Npgsql;
using NUnit.Framework;

namespace IntegrationTests
{
    public abstract class DatabaseSetUp : WebApplicationLifeCycle
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var id = Guid.Parse(DatabaseResources.Id);
            using var connection = new NpgsqlConnection(GetConnectionString());

            connection.Open();

            ExecuteAll(
                connection,
                DatabaseResources.CreateMailsTable,
                DatabaseResources.CreateEmailAddressesTable,
                DatabaseResources.DeleteEmailAddresses,
                DatabaseResources.DeleteMails);

            InsertAll(
                connection,
                id,
                DatabaseResources.InsertMails,
                DatabaseResources.InsertEmailAddresses);

            connection.Close();
        }
        
        private void ExecuteAll(NpgsqlConnection connection, params string[] scripts)
        {
            foreach (var script in scripts) connection.Execute(script);
        }

        private void InsertAll(NpgsqlConnection connection, Guid id, params string[] scripts)
        {
            foreach (var script in scripts)
            {
                using var command = new NpgsqlCommand(script, connection);
                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("date", DateTimeOffset.MinValue);
                command.ExecuteNonQuery();
            }
        }

        protected void Select()
        {
            using var connection = new NpgsqlConnection(GetConnectionString());

            connection.Open();

            ExecuteAll(
                connection,
                DatabaseResources.DeleteEmailAddresses,
                DatabaseResources.DeleteMails);

            connection.Close();
        }

        protected void DeleteAllData()
        {
            using var connection = new NpgsqlConnection(GetConnectionString());

            connection.Open();

            ExecuteAll(
                connection,
                DatabaseResources.DeleteEmailAddresses,
                DatabaseResources.DeleteMails);
            
            connection.Close();
        }

        protected Mail GetLastMail()
        {
            var id = Guid.Parse(DatabaseResources.Id);
            using var connection = new NpgsqlConnection(GetConnectionString());
            connection.Open();


            var mail = connection
                .Query($"SELECT id, subject, body, date, result, failed_message FROM mails WHERE id <> '{id}'")
                .Select(mail => new Mail(
                    mail.subject,
                    mail.body,
                    connection
                        .Query($"SELECT email_address FROM email_addresses WHERE mail_id = '{mail.id}'")
                        .Select(i => i.email_address)
                        .Cast<string>(),
                    DateTimeOffset.ParseExact(mail.date.ToString(), "dd.MM.yyyy H:mm:ss", null, DateTimeStyles.AllowWhiteSpaces), 
                    new Response(mail.result, mail.failed_message))).ToArray();
            connection.Close();

            return mail.FirstOrDefault();
        }
    }
}