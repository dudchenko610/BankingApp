
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Interfaces
{
    public interface IHttpService
    {
        Task<T> GetAsync<T>(string uri, bool authorized = true); // returns default value, if exception occured, otherwise if nothing to return it will be (T) new object()
        Task<T> PostAsync<T>(string uri, object value, bool authorized = true); // returns default value, if exception occured, otherwise if nothing to return it will be (T) new object()
    }
}
