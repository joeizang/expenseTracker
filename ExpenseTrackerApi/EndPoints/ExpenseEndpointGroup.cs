using ExpenseTrackerApi.Abstractions;
using ExpenseTrackerApi.ApiModels;
using ExpenseTrackerApi.DomainModels;
using FluentValidation;

namespace ExpenseTrackerApi.EndPoints;

public static class ExpenseEndpointGroup
{
    public static void MapExpenseEndpoints(this WebApplication app)
    {
        var expenseGroup = app.MapGroup("/expense").AddEndpointFilter(async (context, next) =>
        {
            
        });

        expenseGroup.MapGet("/", async (IValidator<AddExpenseModel> validator,
            IRepository<Expense> repo, CancellationToken token) =>
        {
            var expenses = await repo.GetAllAsync<ExpenseApiModel>().ConfigureAwait(false);
            return Results.Ok(expenses);
        });
    }
    
}