using BankingApp.Api.Validators;
using NUnit.Framework;
using FluentAssertions;
using BankingApp.Shared;
using BankingApp.ViewModels.ViewModels.Authentication;

namespace BankingApp.Api.UnitTests.Validators
{
    [TestFixture]
    public class SignInAuthenticationViewValidatorTests
    {
        private SignInAuthenticationViewValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new SignInAuthenticationViewValidator();
        }

        [Test]
        public void Validate_ValidSignInView_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetValidSignInView());
            validateResult.Errors.Should().BeEmpty();
        }

        [Test]
        public void Validate_EmailIsEmpty_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetSignInViewWithEmptyEmail());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.EmailIsRequired);
        }

        [Test]
        public void Validate_EmailInvalidFormat_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetSignInViewWithInvalidEmail());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.InvalidEmailFormat);
        }

        [Test]
        public void Validate_PasswordIsEmpty_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetSignInViewWithEmptyPassword());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.PasswordRequired);
        }

        [Test]
        public void Validate_PasswordIsShorterThanMinPasswordLengthLetters_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetSignInViewWithShortPassword());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.PasswordIsTooShort);
        }

        [Test]
        public void Validate_PasswordShouldContainUpperCaseLetter_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetSignInViewWithPasswordMissingUpperCaseLetter());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.PasswordUppercaseLetter);
        }

        [Test]
        public void Validate_PasswordShouldContainLowerCaseLetter_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetSignInViewWithPasswordMissingLowerCaseLetter());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.PasswordLowercaseLetter);
        }

        [Test]
        public void Validate_PasswordShouldContainDigit_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetSignInViewWithPasswordMissingDigit());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.PasswordDigit);
        }

        private SignInAuthenticationView GetValidSignInView()
        {
            return new SignInAuthenticationView
            {
                Email = "rusland610@gmail.com",
                Password = "qwerty12345AAA"
            };
        }

        private SignInAuthenticationView GetSignInViewWithInvalidEmail()
        {
            return new SignInAuthenticationView
            {
                Email = "fsdfsdfds",
                Password = "qwerty12345AAA"
            };
        }

        private SignInAuthenticationView GetSignInViewWithEmptyEmail()
        {
            return new SignInAuthenticationView
            {
                Email = "",
                Password = "qwerty12345AAA"
            };
        }

        private SignInAuthenticationView GetSignInViewWithEmptyPassword()
        {
            return new SignInAuthenticationView
            {
                Email = "a@a.com",
                Password = ""
            };
        }

        private SignInAuthenticationView GetSignInViewWithShortPassword()
        {
            return new SignInAuthenticationView
            {
                Email = "a@a.com",
                Password = "aA1"
            };
        }

        private SignInAuthenticationView GetSignInViewWithPasswordMissingUpperCaseLetter()
        {
            return new SignInAuthenticationView
            {
                Email = "a@a.com",
                Password = "qwertyuiopasdffghjk1"
            };
        }

        private SignInAuthenticationView GetSignInViewWithPasswordMissingLowerCaseLetter()
        {
            return new SignInAuthenticationView
            {
                Email = "a@a.com",
                Password = "ADSFDFGDFGDFGDFVDER1"
            };
        }

        private SignInAuthenticationView GetSignInViewWithPasswordMissingDigit()
        {
            return new SignInAuthenticationView
            {
                Email = "a@a.com",
                Password = "ADSFDFGDFGDFGDFVDER"
            };
        }
    }
}
