using ExpenseTrackerApi.ApiModels;
using FluentValidation;

namespace ExpenseTrackerApi.Validators.Expense;

public class UpdateExpenseValidator : AbstractValidator<UpdateExpenseModel>
{
    public UpdateExpenseValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ExpenseDate).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Amount).NotEmpty();
        RuleFor(x => x.Currency).NotEmpty();
        RuleFor(x => x.ExpenseTypes).NotEmpty();
    }
}