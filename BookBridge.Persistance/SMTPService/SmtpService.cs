using System;
using System.Net;
using System.Net.Mail;

namespace BookBridge.Persistance.SMTPService
{
    public sealed class SmtpService
    {
        private const string SenderEmail = "innovationscloudsphere@gmail.com";
        private const string Password = "prne msiu ekyy xqce";

        private const string SmtpAddress = "smtp.gmail.com";
        private const int PortNumber = 587;

        public void SendMessage(string to, string subject, string text)
        {
            if (string.IsNullOrEmpty(to)) throw new ArgumentException("Recipient email is not valid.", nameof(to));
            if (string.IsNullOrEmpty(subject)) throw new ArgumentException("Email subject is not valid.", nameof(subject));
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("Email body is not valid.", nameof(text));

            using var smtpClient = new SmtpClient(SmtpAddress, PortNumber)
            {
                Credentials = new NetworkCredential(SenderEmail, Password),
                EnableSsl = true
            };

            using var mailMessage = new MailMessage(SenderEmail, to)
            {
                Priority = MailPriority.High,
                Subject = subject,
                Body = text,
                IsBodyHtml = true,
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess
            };

            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Exception: {smtpEx.Message}");
                throw new InvalidOperationException("Failed to send email due to SMTP error.", smtpEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send: {ex.Message}");
                throw new InvalidOperationException("Failed to send email.", ex);
            }
        }
    }
}