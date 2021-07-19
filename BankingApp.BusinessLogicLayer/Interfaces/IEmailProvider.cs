using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    public interface IEmailProvider
    {
        Task<bool> SendEmailAsync(string mailTo, string caption, string textMessage);
    }
}
