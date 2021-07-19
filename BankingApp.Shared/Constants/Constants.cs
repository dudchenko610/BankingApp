﻿
namespace BankingApp.Shared
{
    public static class Constants
    {
        public static class Errors
        {
            public static class Deposit
            {
                public const string IncorrectPriceFormat = "Price can't have more than 2 decimal places";
                public const string IncorrectMonthFormat = "Months number should be positive non-zero integer value";
                public const string IncorrectPercentNumber = "Percent should be positive fractional value in range of [1; 100] with two decimal places";

                public const string IncorrectDepositeHistoryId = "DepositeHistoryId should be integer, bigger than one number";
            }

            public static class Page
            {
                public const string IncorrectPageNumberFormat = "Page number should be integer, bigger than one number";
                public const string IncorrectPageSizeFormat = "Page size should be positive integer number, bigger than one number";
            }

            public static class Authentication
            {
                public const string NicknameIsRequired = "Nickname is required";
                public const string EmailIsRequired = "Email is required";
                public const string PasswordIsRequired = "Password is required";

                public const string NicknameLengthIsTooLong = "Nickname name should be less or equal than 12";
                public const string EmailRequired = "Email is required";
                public const string InvalidEmailFormat = "Invalid email format";
                public const string PasswordIsNotHardEnough = "Password is not hard enough";
                public const string PasswordEmpty = "Password is empty";
                public const string PasswordLength = "Password is short";
                public const string PasswordUppercaseLetter = "Password must contain at least one upper case letter";
                public const string PasswordLowercaseLetter = "Password must contain at least one lowercase case letter";
                public const string PasswordDigit = "Password must contain at least one digit";
                public const string PasswordIsTooShort = "Password must be longer than 14 symbols";
                public const string PasswordSpecialCharacter = "Password must contain at least one special character";
                public const string ConfirmPasswordShouldMatchPassword = "Confirm password should match password";
                public const string UserAlreadyExists = "User with such email already exists";
                public const string UerWasNotRegistered = "Unexpected error happened. User was not registered, try again!";
                public const string UserWasNotFound = "User was not found";
                public const string EmailWasNotConfirmed = "Email was not confirmed";
                public const string EmailWasNotDelivered = "Email, was not delivered";

            }
        }

        public static class Routes
        {
            public static class Deposit
            {
                public const string Route = "api/deposit";

                public const string Calculate = "calculateDeposit";
                public const string GetAll = "getAll";
                public const string GetById = "getById";
            }

            public static class Authentication
            {
                public const string Route = "api/authentication";

                public const string SignUp = "signUp";
                public const string SignIn = "signIn";
                public const string ConfirmEmail = "confirmEmail";
            }
        }

        public static class Page
        {
            public const int PageNumber = 1;
            public const int PageSizeElements = 2;
            public const string PagesNumberUrlParam = "pageNumber";
            public const string PageSizeUrlParam = "pageSize";
        }
        
        public static class AppSettings
        {
            public const string EmailConfig = "emailConfig";
            public const string ClientConfig = "clientConfig";
        }
    
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
    }
}
