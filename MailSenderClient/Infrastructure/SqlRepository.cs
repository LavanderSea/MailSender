using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Dapper;
using MailSenderClient.Models;
using Npgsql;

namespace MailSenderClient.Infrastructure
{
    public class SqlRepository : IRepository<Message>
    {
        private readonly string _connectionString;

        public SqlRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        ///     Insert a message into database
        /// </summary>
        public void Insert(Message message)
        {
            var id = Guid.NewGuid();
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var parameters = new Dictionary<string, object>
            {
                {"id", id},
                {"subject", message.Subject},
                {"body", message.Body},
                {"date", message.Date},
                {"result", message.Response.Result},
                {"failed_message", message.Response.FailedMessage}
            };

            Execute(connection, SqlRepositoryResource.InsertMessages, parameters);

            foreach (var recipient in message.Recipients)
                Execute(
                    connection,
                    SqlRepositoryResource.InsertRecipients,
                    new Dictionary<string, object> {{"id", id}, {"email_address", recipient}});

            connection.Close();
        }

        /// <summary>
        ///     Receive all messages from database
        /// </summary>
        public IEnumerable<Message> GetAll()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            var mails = connection
                .Query(SqlRepositoryResource.SelectAllMessages)
                .Select(message => new Message(
                    message.subject,
                    message.body,
                    SelectMessageRecipients(connection, message.id),
                    ParseDate(message.date),
                    new Response(message.result, message.failed_message)))
                .ToArray();

            connection.Close();
            return mails;
        }

        private DateTimeOffset ParseDate(dynamic date)
        {
            DateTimeOffset.TryParseExact(
                date.ToString(),
                "dd.MM.yyyy H:mm:ss",
                null,
                DateTimeStyles.AllowWhiteSpaces,
                out DateTimeOffset dateTime);
            return dateTime;
        }

        private IEnumerable<string> SelectMessageRecipients(IDbConnection connection, dynamic id)
        {
            return connection
                .Query($"SELECT email_address FROM recipients WHERE message_id = '{id}'")
                .Select(i => i.email_address)
                .Cast<string>();
        }

        private void Execute(NpgsqlConnection connection, string sqlScript, Dictionary<string, object> parameters)
        {
            using var command = new NpgsqlCommand(sqlScript, connection);
            foreach (var (name, value) in parameters)
                command.Parameters.AddWithValue(name, value);

            command.ExecuteNonQuery();
        }
    }
}