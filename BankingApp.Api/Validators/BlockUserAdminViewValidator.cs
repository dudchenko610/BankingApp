using BankingApp.Shared;
using BankingApp.ViewModels.Banking.Admin;
using FluentValidation;

namespace BankingApp.Api.Validators
{
    public class BlockUserAdminViewValidator : AbstractValidator<BlockUserAdminView>
    {
        public BlockUserAdminViewValidator()
        {
            RuleFor(d => d.UserId).GreaterThan(0).WithMessage(Constants.Errors.Admin.UserIdOutOfRange);
        }
    }
}
