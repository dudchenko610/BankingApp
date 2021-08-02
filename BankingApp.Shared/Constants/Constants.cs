
namespace BankingApp.Shared
{
    /// <summary>
    /// Shared application constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Error message constants.
        /// </summary>
        public static class Errors
        {
            /// <summary>
            /// Error messages fro deposit controller.
            /// </summary>
            public static class Deposit
            {
                public const string IncorrectPriceFormat = "Price can't have more than 2 decimal places";
                public const string IncorrectMonthFormat = "Months number should be positive non-zero integer value";
                public const string IncorrectPercentNumber = "Percent should be positive fractional value in range of [1; 100] with two decimal places";

                public const string IncorrectDepositeHistoryId = "DepositeHistoryId should be integer, bigger than one number";
                public const string DepositDoesNotBelongsToYou = "This deposit does not belongs to your account";
            }

            /// <summary>
            /// Error messages for paged data queuering.
            /// </summary>
            public static class Page
            {
                public const string IncorrectPageNumberFormat = "Page number should be integer, bigger than one number";
                public const string IncorrectPageSizeFormat = "Page size should be positive integer number, bigger than one number";
            }

            /// <summary>
            /// Error messages for authentication controller.
            /// </summary>
            public static class Authentication
            {
                public const string InvalidNicknameOrPassword = "Invalid nickname or password";
                public const string NicknameIsRequired = "Nickname is required";
                public const string EmailIsRequired = "Email is required";
                public const string EmailFormatIncorrect = "The Email field is not a valid e-mail address.";
                public const string PasswordIsRequired = "Password is required";
                public const string NicknameLengthIsTooLong = "Nickname name should be less or equal than 12";
                public const string InvalidEmailFormat = "Invalid email format";
                public const string PasswordIsNotHardEnough = "Password is not hard enough";
                public const string PasswordEmpty = "Password is empty";
                public const string PasswordUppercaseLetter = "Password must contain at least one upper case letter";
                public const string PasswordLowercaseLetter = "Password must contain at least one lowercase case letter";
                public const string PasswordDigit = "Password must contain at least one digit";
                public const string PasswordIsTooShort = "Password must be longer than 14 symbols";
                public const string PasswordSpecialCharacter = "Password must contain at least one special character";
                public const string ConfirmPasswordShouldMatchPassword = "Confirm password should match password";
                public const string UserAlreadyExists = "User with such email already exists";
                public const string UserWasNotRegistered = "Unexpected error happened. User was not registered, try again!";
                public const string UserWasNotFound = "User was not found";
                public const string EmailWasNotConfirmed = "Email was not confirmed";
                public const string EmailWasNotDelivered = "Email, was not delivered";
                public const string InvalidPassword = "Password is invalid";
                public const string SignInPlease = "Time expired, sign in please again!";
                public const string ErrorWhileSendingMessage = "While sending message error occurred, try latter!";
                public const string UserShouldBelongToOneRole = "User cannot belong to more or less than one role";
                public const string ClientUserWasNotAddedToClientRole = "Client user was not added to client role";
                public const string UserIsBlocked = "Your account was blocked by admin";
                public const string ErrorWhileResetPaswword = "Error happened while resetting password, try later";
            }

            /// <summary>
            /// Error messages for data seeding.
            /// </summary>
            public static class SeedData
            { 
                public const string AdminUserWasNotCreated = "Admin user was not created";
                public const string AdminUserWasNotAddedToAdminRole = "Admin user was not added to admin role";
            }

            /// <summary>
            /// Error messages for admin actions.
            /// </summary>
            public static class Admin
            {
                public const string UserIdOutOfRange = "User id should be greater than 0";
                public const string UnableToBlockUser = "Unable to block user";
            }
        }

        /// <summary>
        /// Controller endpoint routes.
        /// </summary>
        public static class Routes
        {
            /// <summary>
            /// Deposit controller routes.
            /// </summary>
            public static class Deposit
            {
                public const string Route = "api/deposit";
                public const string Calculate = "calculateDeposit";
                public const string GetAll = "getAll";
                public const string GetById = "getById";
            }

            /// <summary>
            /// Authentication controller routes.
            /// </summary>
            public static class Authentication
            {
                public const string Route = "api/authentication";
                public const string SignUp = "signUp";
                public const string SignIn = "signIn";
                public const string ConfirmEmail = "confirmEmail";
                public const string ForgotPassword = "forgotPassword";
                public const string ResetPassword = "resetPassword";
            }

            /// <summary>
            /// Admin controller routes.
            /// </summary>
            public static class Admin
            {
                public const string Route = "api/admin";
                public const string GetAll = "getAll";
                public const string BlockUser = "blockUserw";
            }
        }

        /// <summary>
        /// Constants for paged data queuering.
        /// </summary>
        public static class Page
        {
            public const int PageNumber = 1;
            public const int PageSizeElements = 2;
            public const string PagesNumberUrlParam = "pageNumber";
            public const string PageSizeUrlParam = "pageSize";
        }
        
        /// <summary>
        /// Constants for queuering data from appsetting file.
        /// </summary>
        public static class AppSettings
        {
            public const string EmailConfig = "emailConfig";
            public const string ClientConfig = "clientConfig";
            public const string JwtConfig = "jwtConfig";
            public const string AdminCredentials = "adminCredentials";
        }
    
        /// <summary>
        /// Constants for sending email.
        /// </summary>
        public static class Email
        {
            public const string ParamEmail = "?email=";
            public const string ParamCode = "&code=";
            public const string ClientSMTP = "smtp.gmail.com";
            public const string ConfirmEmail = "Confirm your email";
            public const string ConfirmRegistration = "Click this link for confirm registration:";
            public const string OpenTagLink = "<a href='";
            public const string CloseTagLink = "'>link</a>";
        }

        /// <summary>
        /// Constants for password related actions.
        /// </summary>
        public static class Password
        {
            public const string PasswordValidSymbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            public const int PasswordLength = 14;
            public const int MinPasswordLength = 14;
            public const string PasswordReset = "To reset your password click the link: ";
            public const string PasswordResetHeader = "Reset password";
        }

        /// <summary>
        /// Constants for authentication related actions.
        /// </summary>
        public static class Authentication
        {
            public const string MyApp = "MyApp";
            public const string RefreshToken = "RefreshToken";
            public const string Bearer = "Bearer";
            public const string Token = "Token";
            public const string AccessTokenName = "access_token";
            public const string RefreshTokenName = "refresh_token";
            public const string AuthorizationName = "Authorization";
        }

        /// <summary>
        /// User roles.
        /// </summary>
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Client = "Client";
        }
    }
}
