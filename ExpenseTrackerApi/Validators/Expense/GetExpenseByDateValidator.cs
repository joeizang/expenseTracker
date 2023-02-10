using ExpenseTrackerApi.ApiModels;
using FluentValidation;

namespace ExpenseTrackerApi.Validators.Expense;

public class GetExpenseByDateValidator : AbstractValidator<GetExpenseByDateModel>
{
    public GetExpenseByDateValidator()
    {
        RuleFor(x => x.StartDate).NotEmpty().WithMessage("Start Date is required");
        RuleFor(x => x.StartDate).LessThan(DateTime.Today.AddDays(1))
            .WithMessage("Start Date cannot be in the future");
        RuleFor(x => x.EndDate).NotEmpty().WithMessage("End Date is required");
        RuleFor(x => x.EndDate).LessThan(DateTime.Today.AddDays(1))
            .WithMessage("End Date cannot be in the future");
        RuleFor(x => x.EndDate).LessThan(x => x.StartDate)
            .WithMessage("End Date cannot be before Start Date");
    }
}