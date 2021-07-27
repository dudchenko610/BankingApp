
namespace BankingApp.ViewModels.Banking.Admin
{
    public class UserGetAllAdminViewItem
    {
        public string Nickname { get; set; }
        public string Email { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}
