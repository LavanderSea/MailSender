using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace MailSenderClient
{
    public class GmailSender : ISender
    {
        public GmailSender(string password, string userName)
        {
            _password = password;
            _userName = userName;
        }

        public Response Send(string subject, string body, IEnumerable<string> recipients)
        {
            using var message = new MailMessage();
            foreach (var recipient in recipients) message.To.Add(new MailAddress(recipient));
            message.From = new MailAddress(_userName);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(_userName, _password),
                EnableSsl = true
            };

            try
            {
                client.Send(message);
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

        private readonly string _password;
        private readonly string _userName;
    }
}