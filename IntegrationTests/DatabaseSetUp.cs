using System;
using System.Globalization;
using System.Linq;
using Dapper;
using MailSenderClient.Models;
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
                DatabaseResources.CreateMessagesTable,
                DatabaseResources.CreateRecipientsTable,
                DatabaseResources.DeleteRecipients,
                DatabaseResources.DeleteMessages);

            InsertAll(
                connection,
                id,
                DatabaseResources.InsertMessages,
                DatabaseResources.InsertRecipients);

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

        protected void InsertData()
        {
            var id = Guid.Parse(DatabaseResources.Id);
            using var connection = new NpgsqlConnection(GetConnectionString());

            connection.Open();
            
            InsertAll(
                connection,
                id,
                DatabaseResources.InsertMessages,
                DatabaseResources.InsertRecipients);

            connection.Close();
        }
        
        protected void DeleteAllData()
        {
            using var connection = new NpgsqlConnection(GetConnectionString());

            connection.Open();

            ExecuteAll(
                connection,
                DatabaseResources.DeleteRecipients,
                DatabaseResources.DeleteMessages);

            connection.Close();
        }

        protected Message GetLastMessage()
        {
            var id = Guid.Parse(DatabaseResources.Id);
            using var connection = new NpgsqlConnection(GetConnectionString());
            connection.Open();
            
            var mail = connection
                .Query($"SELECT id, subject, body, date, result, failed_message FROM messages WHERE id <> '{id}'")
                .Select(message => new Message(
                    message.subject,
                    message.body,
                    connection
                        .Query($"SELECT email_address FROM recipients WHERE message_id = '{message.id}'")
                        .Select(i => i.email_address)
                        .Cast<string>(),
                    DateTimeOffset.ParseExact(message.date.ToString(), "dd.MM.yyyy H:mm:ss", null,
                        DateTimeStyles.AllowWhiteSpaces),
                    new Response(message.result, message.failed_message))).ToArray();
            connection.Close();

            return mail.FirstOrDefault();
        }
    }
}