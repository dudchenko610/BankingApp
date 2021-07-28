using BankingApp.Api.Validators;
using NUnit.Framework;
using FluentAssertions;
using BankingApp.Shared;
using BankingApp.ViewModels.ViewModels.Authentication;

namespace BankingApp.Api.UnitTests.Validators
{
    [TestFixture]
    public class ResetPasswordAuthenticationViewValidatorTests
    {
        private ResetPasswordAuthenticationViewValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new ResetPasswordAuthenticationViewValidator();
        }

        [Test]
        public void Validate_ValidResetPasswordView_NoErrorMessage()
        {
            var validateResult = _validator.Validate(GetValidResetPasswordView());
            validateResult.Errors.Should().BeEmpty();
        }

        public void Validate_PasswordIsEmpty_ValidErrorMessage()
        {
            var validateResult = _validator.Validate(GetResetPasswordViewWithEmptyPassword());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.PasswordEmpty);
        }

        [Test]
        public void Validate_PasswordIsShorterThanMinPasswordLengthLetters_ValidErrorMessage()
        {
            var validateResult = _validator.Validate(GetResetPasswordViewWithShortPassword());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.PasswordIsTooShort);
        }

        [Test]
        public void Validate_PasswordShouldContainUpperCaseLetter_ValidErrorMessage()
        {
            var validateResult = _validator.Validate(GetResetPasswordViewWithPasswordMissingUpperCaseLetter());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.PasswordUppercaseLetter);
        }

        [Test]
        public void Validate_PasswordShouldContainLowerCaseLetter_ValidErrorMessage()
        {
            var validateResult = _validator.Validate(GetResetPasswordViewWithPasswordMissingLowerCaseLetter());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.PasswordLowercaseLetter);
        }

        [Test]
        public void Validate_PasswordShouldContainDigit_ValidErrorMessage()
        {
            var validateResult = _validator.Validate(GetResetPasswordViewWithPasswordMissingDigit());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.PasswordDigit);
        }

        private ResetPasswordAuthenticationView GetValidResetPasswordView()
        {
            return new ResetPasswordAuthenticationView
            {
                Password = "qwerty12345AAA",
                ConfirmPassword = "qwerty12345AAA"
            };
        }

        private ResetPasswordAuthenticationView GetResetPasswordViewWithEmptyPassword()
        {
            return new ResetPasswordAuthenticationView
            {
                Password = "",
                ConfirmPassword = ""
            };
        }

        private ResetPasswordAuthenticationView GetResetPasswordViewWithShortPassword()
        {
            return new ResetPasswordAuthenticationView
            {
                Password = "aA1",
                ConfirmPassword = ""
            };
        }

        private ResetPasswordAuthenticationView GetResetPasswordViewWithPasswordMissingUpperCaseLetter()
        {
            return new ResetPasswordAuthenticationView
            {
                Password = "qwertyuiopasdffghjk1",
                ConfirmPassword = "qwertyuiopasdffghjk1"
            };
        }

        private ResetPasswordAuthenticationView GetResetPasswordViewWithPasswordMissingLowerCaseLetter()
        {
            return new ResetPasswordAuthenticationView
            {
                Password = "ADSFDFGDFGDFGDFVDER1",
                ConfirmPassword = "ADSFDFGDFGDFGDFVDER1"
            };
        }

        private ResetPasswordAuthenticationView GetResetPasswordViewWithPasswordMissingDigit()
        {
            return new ResetPasswordAuthenticationView
            {
                Password = "ADSFDFGDFGDFGDFVDER",
                ConfirmPassword = "ADSFDFGDFGDFGDFVDER"
            };
        }
    }
}
