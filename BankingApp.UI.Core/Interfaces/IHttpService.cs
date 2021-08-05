
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Interfaces
{
    public interface IHttpService
    {
        Task<TResult> GetAsync<TResult>(string uri, bool authorized = true);
        Task<TResult> PostAsync<TResult, TModel>(string uri, TModel value, bool authorized = true);
        Task<bool> PostAsync<TModel>(string uri, TModel value, bool authorized = true);
    }
}
