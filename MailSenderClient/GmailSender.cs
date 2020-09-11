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
            using var mail = new MailMessage();
            foreach (var recipient in recipients) mail.To.Add(new MailAddress(recipient));
            mail.From = new MailAddress(_userName);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
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

        private readonly string _password;
        private readonly string _userName;
    }
}