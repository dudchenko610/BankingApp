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
        Task<T> GetAsync<T>(string uri, bool authorized = true);

        /// <summary>
        /// Sends POST request to specified address.
        /// </summary>
        /// <typeparam name="T">Data type of view model should be received.</typeparam>
        /// <param name="uri">Url address.</param>
        /// <param name="value">Object to be serialized and sent as request body.</param>
        /// <param name="authorized">Indicates whether access token should be attached to HTTP request.</param>
        /// <returns>Default value, if exception occured, otherwise, if nothing to return, it will be (T) new object().</returns>
        Task<T> PostAsync<T>(string uri, object value, bool authorized = true);
    }
}


