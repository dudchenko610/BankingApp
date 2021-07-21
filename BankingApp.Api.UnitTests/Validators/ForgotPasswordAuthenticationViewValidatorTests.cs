using BankingApp.Api.Validators;
using BankingApp.ViewModels.Banking.Authentication;
using NUnit.Framework;
using FluentAssertions;
using BankingApp.Shared;

namespace BankingApp.Api.UnitTests.Validators
{
    [TestFixture]
    public class ForgotPasswordAuthenticationViewValidatorTests
    {
        private ForgotPasswordAuthenticationViewValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new ForgotPasswordAuthenticationViewValidator();
        }

        [Test]
        public void Validate_ValidSignInView_NoErrorMessage()
        {
            var validateResult = _validator.Validate(GetValidForgotPasswordView());
            validateResult.Errors.Should().BeEmpty();
        }

        [Test]
        public void Validate_EmailIsEmpty_ValidErrorMessage()
        {
            var validateResult = _validator.Validate(GetForgotPasswordViewWithEmptyEmail());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.EmailIsRequired);
        }

        [Test]
        public void Validate_EmailInvalidFormat_ValidErrorMessage()
        {
            var validateResult = _validator.Validate(GetForgotPasswordViewWithInvalidEmail());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.InvalidEmailFormat);
        }

        private ForgotPasswordAuthenticationView GetValidForgotPasswordView()
        {
            return new ForgotPasswordAuthenticationView
            {
                Email = "rusland610@gmail.com"
            };
        }

        private ForgotPasswordAuthenticationView GetForgotPasswordViewWithEmptyEmail()
        {
            return new ForgotPasswordAuthenticationView
            {
                Email = ""
            };
        }

        private ForgotPasswordAuthenticationView GetForgotPasswordViewWithInvalidEmail()
        {
            return new ForgotPasswordAuthenticationView
            {
                Email = "fsdfsdfds"
            };
        }
    }
}
