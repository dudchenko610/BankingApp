using BankingApp.Api.Validators;
using NUnit.Framework;
using FluentAssertions;
using BankingApp.Shared;
using BankingApp.ViewModels.ViewModels.Authentication;

namespace BankingApp.Api.UnitTests.Validators
{
    public class SignUpAuthenticationViewValidatorTests
    {
        private SignUpAuthenticationViewValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new SignUpAuthenticationViewValidator();
        }

        [Test]
        public void Validate_ValidSignUpView_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetValidSignUpView());
            validateResult.Errors.Should().BeEmpty();
        }

        [Test]
        public void Validate_PasswordsAreNotMatching_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetSignUpViewWithNotMatchingPasswords());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.ConfirmPasswordShouldMatchPassword);
        }

        [Test]
        public void Validate_EmailIsEmpty_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetSignUpViewWithEmptyEmail());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.EmailIsRequired);
        }

        [Test]
        public void Validate_EmailInvalidFormat_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetSignUpViewWithInvalidEmail());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.InvalidEmailFormat);
        }

        [Test]
        public void Validate_NickNameIsLongerThan12Symbols_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetSignUpViewWithTooLongNickname());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.NicknameLengthIsTooLong);
        }

        [Test]
        public void Validate_EmptyNickname_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetSignUpViewWithEmptyNickname());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.NicknameIsRequired);
        }

        [Test]
        public void Validate_PasswordIsEmpty_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetSignUpViewWithEmptyPassword());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.PasswordRequired);
        }

        [Test]
        public void Validate_PasswordIsShorterThanMinPasswordLengthLetters_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetSignUpViewWithShortPassword());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.PasswordIsTooShort);
        }

        [Test]
        public void Validate_PasswordShouldContainUpperCaseLetter_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetSignUpViewWithPasswordMissingUpperCaseLetter());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.PasswordUppercaseLetter);
        }

        [Test]
        public void Validate_PasswordShouldContainLowerCaseLetter_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetSignUpViewWithPasswordMissingLowerCaseLetter());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.PasswordLowercaseLetter);
        }

        [Test]
        public void Validate_PasswordShouldContainDigit_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetSignUpViewWithPasswordMissingDigit());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.PasswordDigit);
        }

        private SignUpAuthenticationView GetValidSignUpView()
        {
            return new SignUpAuthenticationView
            {
                Email = "rusland610@gmail.com",
                Nickname = "Bobik",
                Password = "qwerty12345AAA",
                ConfirmPassword = "qwerty12345AAA"
            };
        }

        private SignUpAuthenticationView GetSignUpViewWithNotMatchingPasswords()
        {
            return new SignUpAuthenticationView
            {
                Email = "rusland610@gmail.com",
                Nickname = "Bobik",
                Password = "qwerty12345AAA",
                ConfirmPassword = "qwerty"
            };
        }

        private SignUpAuthenticationView GetSignUpViewWithEmptyEmail()
        {
            return new SignUpAuthenticationView
            {
                Email = "",
                Nickname = "Bobik",
                Password = "qwerty12345AAA",
                ConfirmPassword = "qwerty"
            };
        }

        private SignUpAuthenticationView GetSignUpViewWithInvalidEmail()
        {
            return new SignUpAuthenticationView
            {
                Email = "fsdfsdfds",
                Nickname = "Bobik",
                Password = "qwerty12345AAA",
                ConfirmPassword = "qwerty"
            };
        }

        private SignUpAuthenticationView GetSignUpViewWithTooLongNickname()
        {
            return new SignUpAuthenticationView
            {
                Email = "a@a.com",
                Nickname = "qwertyuiop12dfdsfsfs3",
                Password = "qwerty12345AAA",
                ConfirmPassword = "qwerty"
            };
        }

        private SignUpAuthenticationView GetSignUpViewWithEmptyNickname()
        {
            return new SignUpAuthenticationView
            {
                Email = "a@a.com",
                Nickname = "",
                Password = "qwerty12345AAA",
                ConfirmPassword = "qwerty"
            };
        }

        private SignUpAuthenticationView GetSignUpViewWithEmptyPassword()
        {
            return new SignUpAuthenticationView
            {
                Email = "a@a.com",
                Nickname = "bfgdgdds",
                Password = "",
                ConfirmPassword = ""
            };
        }

        private SignUpAuthenticationView GetSignUpViewWithShortPassword()
        {
            return new SignUpAuthenticationView
            {
                Email = "a@a.com",
                Nickname = "bfgdgdds",
                Password = "aA1",
                ConfirmPassword = ""
            };
        }

        private SignUpAuthenticationView GetSignUpViewWithPasswordMissingUpperCaseLetter()
        {
            return new SignUpAuthenticationView
            {
                Email = "a@a.com",
                Nickname = "bfgdgdds",
                Password = "qwertyuiopasdffghjk1",
                ConfirmPassword = "qwertyuiopasdffghjk1"
            };
        }

        private SignUpAuthenticationView GetSignUpViewWithPasswordMissingLowerCaseLetter()
        {
            return new SignUpAuthenticationView
            {
                Email = "a@a.com",
                Nickname = "bfgdgdds",
                Password = "ADSFDFGDFGDFGDFVDER1",
                ConfirmPassword = "ADSFDFGDFGDFGDFVDER1"
            };
        }

        private SignUpAuthenticationView GetSignUpViewWithPasswordMissingDigit()
        {
            return new SignUpAuthenticationView
            {
                Email = "a@a.com",
                Nickname = "bfgdgdds",
                Password = "ADSFDFGDFGDFGDFVDER",
                ConfirmPassword = "ADSFDFGDFGDFGDFVDER"
            };
        }
    }
}
