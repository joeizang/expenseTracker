using ExpenseTrackerApi.Abstractions;
using ExpenseTrackerApi.ApiModels;
using ExpenseTrackerApi.DomainModels;
using ExpenseTrackerApi.Validators.Expense;
using FluentValidation;

namespace ExpenseTrackerApi.EndPoints;

public static class ExpenseEndpointGroup
{
    public static void MapExpenseEndpoints(this WebApplication app)
    {
        var expenseGroup = app.MapGroup("/api");

        expenseGroup.MapGet("/expense", async (IRepository<Expense> repo) =>
        {
            var expenses = await repo.GetAllAsync<ExpenseApiModel>().ConfigureAwait(false);
            return Results.Ok(expenses);
        });
    }
    
}