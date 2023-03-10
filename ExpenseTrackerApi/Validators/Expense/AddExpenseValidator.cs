using ExpenseTrackerApi.ApiModels;
using FluentValidation;

namespace ExpenseTrackerApi.Validators.Expense;

public class AddExpenseValidator : AbstractValidator<AddExpenseModel>
{
    public AddExpenseValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(300)
            .WithMessage("Every expense must have a description");
        RuleFor(x => x.ExpenseDate)
            .NotEmpty()
            .WithMessage("Every expense must have a date");
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Every expense must have a positive amount");
        RuleFor(x => x.Currency)
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("Every expense must have a currency");
        RuleFor(x => x.ExpenseTypes)
            .NotEmpty()
            .WithMessage("Every expense must have at least one expense type");
    }
}