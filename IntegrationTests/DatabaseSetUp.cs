using System;
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
            var id = Guid.Parse(DatabaseResource.Id);
            using var connection = new NpgsqlConnection(GetConnectionString());

            connection.Open();

            ExecuteAll(
                connection,
                DatabaseResource.CreateMailsTable,
                DatabaseResource.CreateEmailAddressesTable,
                DatabaseResource.DeleteEmailAddresses,
                DatabaseResource.DeleteMails);

            InsertAll(
                connection,
                id,
                DatabaseResource.InsertMails,
                DatabaseResource.InsertEmailAddresses);

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
                command.ExecuteNonQuery();
            }
        }

        protected void Select()
        {
            using var connection = new NpgsqlConnection(GetConnectionString());

            connection.Open();

            ExecuteAll(
                connection,
                DatabaseResource.DeleteEmailAddresses,
                DatabaseResource.DeleteMails);

            connection.Close();
        }

        protected void DeleteAllData()
        {
            using var connection = new NpgsqlConnection(GetConnectionString());

            connection.Open();

            ExecuteAll(
                connection,
                DatabaseResource.DeleteEmailAddresses,
                DatabaseResource.DeleteMails);
            
            connection.Close();
        }

        protected Mail GetLastMail()
        {
            var id = Guid.Parse(DatabaseResource.Id);
            using var connection = new NpgsqlConnection(GetConnectionString());
            connection.Open();


            var mail = connection
                .Query($"SELECT id, subject, body, result, failed_message FROM mails WHERE id <> '{id}'")
                .Select(mail => new Mail(
                    mail.subject,
                    mail.body,
                    connection
                        .Query($"SELECT email_address FROM email_addresses WHERE mail_id = '{mail.id}'")
                        .Select(i => i.email_address)
                        .Cast<string>(),
                    new Response(mail.result, mail.failed_message))).ToArray();
            connection.Close();

            return mail.FirstOrDefault();
        }
    }
}