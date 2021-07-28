
namespace BankingApp.ViewModels.Banking.Admin
{
    public class UserGetAllAdminViewItem
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}
