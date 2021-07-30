using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Allows the user to send email messages to the specified address.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends email to the specified address.
        /// </summary>
        /// <param name="mailTo">Email address of receiver.</param>
        /// <param name="subject">Message header.</param>
        /// <param name="messageBody">Message content.</param>
        /// <returns>If sending message was succeed returns true, otherwise false.</returns>
        Task<bool> SendEmailAsync(string mailTo, string subject, string messageBody);
    }
}
