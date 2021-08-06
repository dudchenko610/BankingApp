
namespace BankingApp.Shared.Options
{
    /// <summary>
    /// View model fro jwt options.
    /// </summary>
    public class JwtConnectionOptions
    {
        /// <summary>
        /// Provider of access token.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Receivers of access token.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Access token lifetime.
        /// </summary>
        public int Lifetime { get; set; }

        /// <summary>
        /// Secret key for generation access token.
        /// </summary>
        public string SecretKey { get; set; }
    }
}
