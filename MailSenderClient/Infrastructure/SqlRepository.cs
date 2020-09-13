using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MailSenderClient.Infrastructure
{
    public class SqlRepository : IRepository<Mail>
    {
        public SqlRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Set(Mail mail)
        {
            var id = Guid.NewGuid();
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var command =
                new NpgsqlCommand(
                    "INSERT INTO mails(id, subject, body, date, result, failed_message) VALUES(@id, @subject, @body, @date, @result, @failed_message);",
                    connection);
            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("subject", mail.Subject);
            command.Parameters.AddWithValue("body", mail.Body);
            command.Parameters.AddWithValue("date", mail.Date);
            command.Parameters.AddWithValue("result", mail.Response.Result);
            command.Parameters.AddWithValue("failed_message", mail.Response.FailedMessage);
            command.ExecuteNonQuery();

            foreach (var recipient in mail.Recipients)
            {
                using var command2 = new NpgsqlCommand("INSERT INTO email_addresses(mail_id, email_address) VALUES(@id, @email_address);",
                    connection);
                command2.Parameters.AddWithValue("id", id);
                command2.Parameters.AddWithValue("email_address", recipient);
                command2.ExecuteNonQuery();
            }

            connection.Close();
        }

        public IEnumerable<Mail> GetAll()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            
            var mails = connection
                .Query("SELECT id, subject, body, date, result, failed_message FROM mails")
                .Select(mail => new Mail(
                    mail.subject,
                    mail.body,
                    connection
                        .Query($"SELECT email_address FROM email_addresses WHERE mail_id = '{mail.id}'")
                        .Select(i => i.email_address)
                        .Cast<string>(),
                    DateTimeOffset.ParseExact(mail.date.ToString(), "dd.MM.yyyy H:mm:ss", null,  DateTimeStyles.AllowWhiteSpaces),
                    new Response(mail.result, mail.failed_message))).ToArray();

            connection.Close();
            return mails;
        }

        private readonly string _connectionString;
    }
}