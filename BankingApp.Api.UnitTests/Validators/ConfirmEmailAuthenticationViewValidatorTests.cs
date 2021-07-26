using BankingApp.Api.Validators;
using NUnit.Framework;
using FluentAssertions;
using BankingApp.ViewModels.Banking.Authentication;
using BankingApp.Shared;

namespace BankingApp.Api.UnitTests.Validators
{
    [TestFixture]
    public class ConfirmEmailAuthenticationViewValidatorTests
    {
        private ConfirmEmailAuthenticationViewValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new ConfirmEmailAuthenticationViewValidator();
        }

        [Test]
        public void Validate_ValidConfirmEmailView_NoErrorMessage()
        {
            var validateResult = _validator.Validate(GetValidConfirmEmailView());
            validateResult.Errors.Should().BeEmpty();
        }

        [Test]
        public void Validate_EmailIsEmpty_ValidErrorMessage()
        {
            var validateResult = _validator.Validate(GetConfirmEmailViewWithEmptyEmail());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.EmailIsRequired);
        }

        [Test]
        public void Validate_EmailInvalidFormat_ValidErrorMessage()
        {
            var validateResult = _validator.Validate(GetConfirmEmailViewWithInvalidEmail());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.InvalidEmailFormat);
        }

        private ConfirmEmailAuthenticationView GetValidConfirmEmailView()
        {
            return new ConfirmEmailAuthenticationView
            {
                Email = "rusland610@gmail.com"
            };
        }

        private ConfirmEmailAuthenticationView GetConfirmEmailViewWithEmptyEmail()
        {
            return new ConfirmEmailAuthenticationView
            {
                Email = ""
            };
        }

        private ConfirmEmailAuthenticationView GetConfirmEmailViewWithInvalidEmail()
        {
            return new ConfirmEmailAuthenticationView
            {
                Email = "fsdfsdfds"
            };
        }
    }
}
