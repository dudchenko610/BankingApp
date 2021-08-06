using System.Threading.Tasks;

namespace BankingApp.UI.Core.Interfaces
{
    /// <summary>
    /// Used to simplify http requests usage.
    /// </summary>
    public interface IHttpService
    {
        /// <summary>
        /// Sends GET request to specified address.
        /// </summary>
        /// <typeparam name="T">Data type of view model should be received.</typeparam>
        /// <param name="uri">Url address.</param>
        /// <param name="authorized">Indicates whether access token should be attached to HTTP request.</param>
        /// <returns>Default value, if exception occured, otherwise, if nothing to return, it will be (T) new object().</returns>
        Task<TResult> GetAsync<TResult>(string uri, bool authorized = true);

        /// <summary>
        /// Sends POST request to specified address.
        /// </summary>
        /// <typeparam name="T">Data type of view model should be received.</typeparam>
        /// <param name="uri">Url address.</param>
        /// <param name="value">Object to be serialized and sent as request body.</param>
        /// <param name="authorized">Indicates whether access token should be attached to HTTP request.</param>
        /// <returns>Default value, if exception occured, otherwise, if nothing to return, it will be (T) new object().</returns>
        Task<TResult> PostAsync<TResult, TModel>(string uri, TModel value, bool authorized = true);
        Task<bool> PostAsync<TModel>(string uri, TModel value, bool authorized = true);
    }
}


