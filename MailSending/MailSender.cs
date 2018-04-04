using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using SendGrid;
using System.Text;
using System.Threading.Tasks;

namespace MailSending
{
    class MailSender
    {
        static void Main(string[] args)
        {
            SmtpClient client =  CreateSMTPClient(
                "smtp.gmail.com", 
                587, 
                new NetworkCredential { UserName = "harser09@gmail.com", Password = "CogitoErgoSum124C41+" }, 
                true);

            MailMessage message =  CreateMail(
                new MailAddress("harser09@gmail.com"),
                new MailAddress("vardanyangogor95@gmail.com"),
                "Hey Grigor",
                "Test"
                );

            sendMail(client, message);
            Console.WriteLine("Mail Message Sent.");

            Console.WriteLine("Done!!!");
            Console.Read();
        }


        #region Standard E-Mail Sending

        private static SmtpClient CreateSMTPClient(string host, int port, NetworkCredential credentials, bool SSLIsTrue)
        {          
            
            SmtpClient smtpClient = new SmtpClient
            {      
                Host = host,                
                Port = port,
                Credentials = credentials,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = SSLIsTrue
            };

            Console.WriteLine("SMTP Client created.");
            return smtpClient;
        }

        private static MailMessage CreateMail(MailAddress From, MailAddress To, string Content, string subject, MailAddress CC = null)
        {
            MailMessage mail = new MailMessage();

            mail.From = From;
            mail.To.Add(To);

            mail.Body = Content;
            mail.Subject = subject;

            if(CC != null)
                mail.CC.Add(CC);

            Console.WriteLine("Mail Message created.");
            return mail;
        }


        private static void sendMail(SmtpClient client, MailMessage mail)
        {
            client.Send(mail);
        }

        #endregion
    }
}

