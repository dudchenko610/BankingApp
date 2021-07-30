namespace BankingApp.ViewModels.ViewModels.Authentication
{
    /// <summary>
    /// View model used to confirm the user's email.
    /// </summary>
    public class ConfirmEmailAuthenticationView
    {
        /// <summary>
        /// User email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Email conformation token.
        /// </summary>
        public string Code { get; set; }
    }
}
