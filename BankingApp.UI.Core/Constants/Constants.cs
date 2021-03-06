
namespace BankingApp.UI.Core.Constants
{
    /// <summary>
    /// Client side constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Notifications used to inform user about some event happened.
        /// </summary>
        public static class Notifications
        {
            public const string ConfirmYourEmail = "To confirm email, you need to click a link in sent message on your mailbox";
            public const string UnexpectedError = "Unexpected error happened, try later!";
            public const string Unauthorized = "Your sign in session expired, sign in again please";
            public const string EmailSuccessfullyConfirmed = "Email was successully confirmed, please login now";
            public const string SignInSuccess = "You are signed in!";
            public const string PasswordResetSuccessfully = "Password was reset successfully, sign in using new password";
            public const string PasswordResetEmailSent = "Reset link was sent to your mailbox";

            public const string UserSuccessfullyBlocked = "User successfully blocked";
            public const string UserSuccessfullyUnblocked = "User successfully unblocked";
            public const string ErrorWhileBlockingUser = "Error while blocking user";
            public const string ErrorWhileUnblockingUser = "Error while unblocking user";
        }

        /// <summary>
        /// Client side page routes.
        /// </summary>
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
            public const string ForgotPasswordPage = "/forgotPassword";
            public const string UsersPage = "/users";
        }

        /// <summary>
        /// Keywords used by authentication system.
        /// </summary>
        public static class Authentication
        {
            public const string TokensView = "tokens";
        }
    }
}
