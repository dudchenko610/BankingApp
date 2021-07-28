using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string mailTo, string subject, string messageBody);
    }
}
