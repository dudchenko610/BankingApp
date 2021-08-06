
namespace BankingApp.Shared.Options
{
    /// <summary>
    /// View model for client connection options.
    /// </summary>
    public class ClientConnectionOptions
    {
        /// <summary>
        /// Url of client application server.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Url of confirm email page on client server.
        /// </summary>
        public string ConfirmPath { get; set; }

        /// <summary>
        /// Url of reset password page on client server.
        /// </summary>
        public string ResetPath { get; set; }
    }
}
