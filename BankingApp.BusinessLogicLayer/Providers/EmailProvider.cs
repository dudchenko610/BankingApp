using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.Shared;
using BankingApp.Shared.Options;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Providers
{
    [ExcludeFromCodeCoverage]
    public class EmailProvider : IEmailProvider
    {
        private readonly EmailConnectionOptions _emailConnectionOptions;

        public EmailProvider(IOptions<EmailConnectionOptions> emailConnectionOptions)
        {
            _emailConnectionOptions = emailConnectionOptions.Value;
        }

        public async Task<bool> SendEmailAsync(string mailTo, string caption, string textMessage)
        {
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress(_emailConnectionOptions.MailAddress);
                mail.To.Add(new MailAddress(mailTo));
                mail.Subject = caption;
                mail.Body = textMessage;

                var client = new SmtpClient();

                client.Host = Constants.Email.ClientSMTP;
                client.Port = Convert.ToInt32(_emailConnectionOptions.Port);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_emailConnectionOptions.MailAddress, _emailConnectionOptions.Password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                await client.SendMailAsync(mail);
                mail.Dispose();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
