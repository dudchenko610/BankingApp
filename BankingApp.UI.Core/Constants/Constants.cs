
namespace BankingApp.UI.Core.Constants
{
    public static class Constants
    {
        public static class Notifications
        {
            public const string ConfirmYourEmail = "To confirm email, you need to click a link in sent message on your mailbox";
            public const string UnexpectedError = "Unexpected error happened, try later!";
            public const string EmailSuccessfullyConfirmed = "Email was successully confirmed, please login now";
            public const string SignInSuccess = "You are signed in!";
            public const string PasswordResetSuccessfully = "Password was reset successfully, your new password was sent to email";
        }

        public static class Routes
        {
            public const string MainPage = "/";
            public const string HistoryPage = "/history";
            public const string DetailsPage = "/details";
            public const string NotificationPage = "/notification";
            public const string SignUpPage = "/signUp";
            public const string SignInPage = "/signIn";
            public const string LogoutPage = "/logout";
            public const string ResetPasswordPage = "/resetPassword";
        }

        public static class Authentication
        {
            public const string TokensView = "tokens";
        }
    }
}
