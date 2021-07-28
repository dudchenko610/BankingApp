using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.Shared;
using BankingApp.Shared.Options;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Services
{
    [ExcludeFromCodeCoverage]
    public class EmailService : IEmailService
    {
        private readonly EmailConnectionOptions _emailConnectionOptions;

        public EmailService(IOptions<EmailConnectionOptions> emailConnectionOptions)
        {
            _emailConnectionOptions = emailConnectionOptions.Value;
        }

        public async Task<bool> SendEmailAsync(string mailTo, string subject, string messageBody)
        {
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress(_emailConnectionOptions.MailAddress);
                mail.To.Add(new MailAddress(mailTo));
                mail.Subject = subject;
                mail.Body = messageBody;

                var client = new SmtpClient();

                client.Host = Constants.Email.ClientSMTP;
                client.Port = Convert.ToInt32(_emailConnectionOptions.Port);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_emailConnectionOptions.MailAddress, _emailConnectionOptions.Password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                await client.SendMailAsync(mail);
                mail.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
