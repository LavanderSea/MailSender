using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MailSenderClient.Infrastructure
{
    public class SqlRepository : IRepository<Message>
    {
        public SqlRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Set(Message message)
        {
            var id = Guid.NewGuid();
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var command =
                new NpgsqlCommand(
                    "INSERT INTO messages(id, subject, body, result, failed_message) VALUES(@id, @subject, @body, @result, @failed_message);",
                    connection);
            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("subject", message.Subject);
            command.Parameters.AddWithValue("body", message.Body);
            command.Parameters.AddWithValue("result", message.Response.Result);
            command.Parameters.AddWithValue("failed_message", message.Response.FailedMessage);
            command.ExecuteNonQuery();

            foreach (var recipient in message.Recipients)
            {
                using var command2 = new NpgsqlCommand("INSERT INTO emails(message_id, email) VALUES(@id, @email);",
                    connection);
                command2.Parameters.AddWithValue("id", id);
                command2.Parameters.AddWithValue("email", recipient);
                command2.ExecuteNonQuery();
            }

            connection.Close();
        }

        public IEnumerable<Message> GetAll()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var messages = connection
                .Query("SELECT id, subject, body, result, failed_message FROM Messages")
                .Select(message => new Message(
                    message.subject,
                    message.body,
                    connection
                        .Query($"SELECT email FROM Emails WHERE message_id = '{message.id}'")
                        .Select(i => i.email)
                        .Cast<string>(),
                    new Response(message.result, message.failed_message))).ToArray();
            connection.Close();
            return messages;
        }

        private readonly string _connectionString;
    }
}