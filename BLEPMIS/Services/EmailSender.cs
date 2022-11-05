using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BLEPMIS.Services
{
    public class EmailSender : IEmailSender
    {
        private string _smtpServer;
        private int _port;
        private string _username;
        private string _password;
        private string _displayName;

        public EmailSender(string smtpServer, int port, string password, string displayName, string host)
        {
            _smtpServer = smtpServer;
            _port = port;
            _username = host;
            _password = password;
            _displayName = displayName;
        }
        public bool SendEmail(string emailAddress, string subject, string text)
        {
            /*using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(emailAddress));
                message.CC.Add(new MailAddress("kafil.fast09@gmail.com"));
                message.Subject = subject;
                message.Body = text;
                message.IsBodyHtml = true;
                message.From = new MailAddress(_username, _displayName);
                using (var client = new SmtpClient(_smtpServer))
                {
                    client.Port = _port;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(_username, _password);
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }*/
            MimeMessage message = new MimeMessage();
            MailboxAddress from = new MailboxAddress(_displayName,_username);            
            MailboxAddress to = new MailboxAddress("BLEP",emailAddress);
            MailboxAddress cc = new MailboxAddress("Kafil","kafil.fast09@gmail.com");
            message.From.Add(from);
            message.To.Add(to);
            message.Cc.Add(cc);
            message.Subject = subject;
            BodyBuilder bodyBuilder = new BodyBuilder();
            //bodyBuilder.HtmlBody = "<h1>Hello World!</h1>";
            //bodyBuilder.TextBody = text;
            bodyBuilder.HtmlBody = text;
            message.Body = bodyBuilder.ToMessageBody();

            SmtpClient client = new SmtpClient();
            client.Connect(_smtpServer, _port, false);
            client.Authenticate(_username, _password);
            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
            return true;
        }
    }
}
