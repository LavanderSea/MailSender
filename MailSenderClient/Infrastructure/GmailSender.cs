using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using MailSenderClient.Models;

namespace MailSenderClient.Infrastructure
{
    public class GmailSender : ISender
    {
        private readonly string _password;
        private readonly int _port;
        private readonly string _userName;

        public GmailSender(string password, string userName, int port)
        {
            _password = password;
            _userName = userName;
            _port = port;
        }

        /// <summary>
        ///     Send a message and receive result: status of sending ("Ok or "Failed") and message about failing (empty field if
        ///     sending succeed)
        /// </summary>
        public Response Send(string subject, string body, IEnumerable<string> recipients)
        {
            using var mail = new MailMessage();
            foreach (var recipient in recipients) mail.To.Add(new MailAddress(recipient));
            mail.From = new MailAddress(_userName);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            var client = new SmtpClient("smtp.gmail.com")
            {
                Port = _port,
                Credentials = new NetworkCredential(_userName, _password),
                EnableSsl = true
            };

            try
            {
                client.Send(mail);
            }
            catch (Exception exception)
            {
                return new Response("Failed", exception.Message);
            }
            finally
            {
                client.Dispose();
            }

            return new Response("Ok", string.Empty);
        }
    }
}