using System;
using System.Net;
using System.Net.Mail;

namespace CLManager
{
    public class SendEmail
    {
        public string to = "";
        public string from = "";
        public string password = "";
        public string server = "smtp.gmail.com";
        public string subject = "";
        public string body = "";
        public string attach = "";
        public string fromName = "";
        public string toName = "";
        public int port = 587;
        public string login = "";
        public int timeout = 20000;

        public void Send()
        {
            try
            {
                if (fromName == "")
                    fromName = from;
                if (toName == "")
                    toName = to;
                if (login == "")
                    login = from;
                var fromAddress = new MailAddress(from, fromName);
                var toAddress = new MailAddress(to, toName);

                var smtp = new SmtpClient
                {
                    Host = server,
                    Port = port,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(login, password),
                    Timeout = timeout
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    if (attach != "")
                        message.Attachments.Add(new Attachment(attach));
                    smtp.Send(message);
                }
                Console.WriteLine("OK!");
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                Console.ReadLine();
            }
        }
    }
}
