using BankingApp.Api.Validators;
using BankingApp.ViewModels.Banking.Admin;
using NUnit.Framework;
using FluentAssertions;
using BankingApp.Shared;

namespace BankingApp.Api.UnitTests.Validators
{
    public class BlockUserAdminViewValidatorTests
    {
        private BlockUserAdminViewValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new BlockUserAdminViewValidator();
        }

        [Test]
        public void Validate_UserIdLessThanZero_ValidErrorMessage()
        {
            var validateResult = _validator.Validate(GetBlockUserAdminViewWithInvalidUserId());
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Admin.UserIdOutOfRange);
        }

        private BlockUserAdminView GetBlockUserAdminViewWithInvalidUserId()
        {
            return new BlockUserAdminView
            {
                UserId = -1,
                Block = true
            };
        }
    }
}
