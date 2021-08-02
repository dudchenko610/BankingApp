
namespace BankingApp.Shared.Options
{
    /// <summary>
    /// View models for email connection options.
    /// </summary>
    public class EmailConnectionOptions
    {
        /// <summary>
        /// Email address of sender.
        /// </summary>
        public string MailAddress { get; set; }

        /// <summary>
        /// Password of sender email.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Port of sender.
        /// </summary>
        public string Port { get; set; }
    }
}
