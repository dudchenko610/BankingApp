using BankingApp.Api.Validators;
using NUnit.Framework;
using FluentAssertions;
using BankingApp.Shared;
using BankingApp.ViewModels.ViewModels.Authentication;

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
        public void Validate_ValidForgotPasswordView_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetForgotPasswordView("rusland610@gmail.com"));
            validateResult.Errors.Should().BeEmpty();
        }

        [Test]
        public void Validate_EmailIsEmpty_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetForgotPasswordView(string.Empty));
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.EmailIsRequired);
        }

        [Test]
        public void Validate_EmailInvalidFormat_ExpectedResults()
        {
            var validateResult = _validator.Validate(GetForgotPasswordView("fsdfsdfds"));
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Authentication.InvalidEmailFormat);
        }

        private ForgotPasswordAuthenticationView GetForgotPasswordView(string email)
        {
            return new ForgotPasswordAuthenticationView
            {
                Email = email
            };
        }
    }
}
