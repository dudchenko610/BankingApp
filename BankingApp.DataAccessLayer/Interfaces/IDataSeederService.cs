using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Interfaces
{
    public interface IDataSeederService
    {
        Task SeedDataAsync();
    }
}
