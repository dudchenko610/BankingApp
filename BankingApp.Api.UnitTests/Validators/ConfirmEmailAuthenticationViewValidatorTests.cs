using BankingApp.Api.Validators;
using NUnit.Framework;
using FluentAssertions;
using BankingApp.Shared;
using BankingApp.ViewModels.ViewModels.Authentication;

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
        public void Validate_ValidConfirmEmailView_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetConfirmEmailView("rusland610@gmail.com"));
            validateResult.Errors.Should().BeEmpty();
        }

        [Test]
        public void Validate_EmailIsEmpty_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetConfirmEmailView(string.Empty));
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.EmailIsRequired);
        }

        [Test]
        public void Validate_EmailInvalidFormat_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetConfirmEmailView("fsdfsdfds"));
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.InvalidEmailFormat);
        }

        private ConfirmEmailAuthenticationView GetConfirmEmailView(string email)
        {
            return new ConfirmEmailAuthenticationView
            {
                Email = email
            };
        }
    }
}
