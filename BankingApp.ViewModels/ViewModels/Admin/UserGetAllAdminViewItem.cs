
namespace BankingApp.ViewModels.Banking.Admin
{
    /// <summary>
    /// View model item used to represent user in admin user list.
    /// </summary>
    public class UserGetAllAdminViewItem
    {
        /// <summary>
        /// User id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User nickname.
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// User email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Specifies if the user is blocked.
        /// </summary>
        public bool IsBlocked { get; set; }

        /// <summary>
        /// Specifies if the user's email is confirmed.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }
    }
}
