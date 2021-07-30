using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Initializes storage with initial data.
    /// </summary>
    public interface IDataSeederService
    {
        /// <summary>
        /// Inserts initial data in storage
        /// </summary>
        Task SeedDataAsync();
    }
}
