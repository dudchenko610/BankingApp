
namespace BankingApp.ViewModels.Banking.Admin
{
    /// <summary>
    /// View model used to represent user to block / unblock.
    /// </summary>
    public class BlockUserAdminView
    {
        /// <summary>
        /// Id of user to block / unblock.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Type of action (block / unblock).
        /// </summary>
        public bool Block { get; set; }
    }
}
