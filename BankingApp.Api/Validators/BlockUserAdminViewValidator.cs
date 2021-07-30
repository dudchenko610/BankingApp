using BankingApp.Shared;
using BankingApp.ViewModels.Banking.Admin;
using FluentValidation;

namespace BankingApp.Api.Validators
{
    /// <summary>
    /// Validates properties of <see cref="BlockUserAdminView"/>.
    /// </summary>
    public class BlockUserAdminViewValidator : AbstractValidator<BlockUserAdminView>
    {
        /// <summary>
        /// Assigns rules for validating properties of <see cref="BlockUserAdminView"/>.
        /// </summary>
        public BlockUserAdminViewValidator()
        {
            RuleFor(d => d.UserId).GreaterThan(0).WithMessage(Constants.Errors.Admin.UserIdOutOfRange);
        }
    }
}
