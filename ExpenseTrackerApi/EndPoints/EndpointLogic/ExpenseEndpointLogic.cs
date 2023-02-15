using ExpenseTrackerApi.Abstractions;
using ExpenseTrackerApi.ApiModels;
using ExpenseTrackerApi.DomainModels;
using ExpenseTrackerApi.DomainModels.ValueObjects;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.EndPoints.EndpointLogic;

public static class ExpenseEndpointLogic
{
    public static async Task<IResult> GetExpenses([FromServices] IRepository<Expense> repo)
    {
        try
        {
            var expenses = await repo.GetAllAsync<ExpenseApiModel>().ConfigureAwait(false);
            return Results.Ok(expenses);
        }
        catch (Exception)
        {
            return Results.Problem("There was a problem. But its not your fault", statusCode: 500);
        }
    }
    public static async Task<IResult> GetExpenseById(Guid id, [FromServices] IExpenseRepository repo)
    {
        try
        {
            var expense = await repo.GetAsync<ExpenseApiModel>(id).ConfigureAwait(false);
            return Results.Ok(expense);
        }
        catch (Exception)
        {
            return Results.Problem("There was a problem retrieving the expense", statusCode: 500);
        }
    }
    public static async Task<IResult> GetExpenseByDate(DateTime date, [FromServices] IExpenseRepository repo)
    {
        try
        {
            var expenses = await repo.GetByDateAsync<ExpenseApiModel>(date)
                .ConfigureAwait(false);
            var expenseApiModels = expenses as ExpenseApiModel[] ?? expenses.ToArray();
            return !expenseApiModels.Any()
                ? Results.BadRequest(
                    new
                    {
                        Error = "There seems to be a problem with the date submitted"
                    })
                : Results.Ok(expenseApiModels);
        }
        catch (Exception)
        {
            return Results.Problem("There was a problem but its not your fault!", statusCode: 500);
        }
    }
    public static async Task<IResult> GetExpenseByDescription(string description, [FromServices] IExpenseRepository repo)
    {
        try
        {
            var expenses = await repo.GetManyByDescriptionAsync(description)
                .ConfigureAwait(false);
            return Results.Ok(expenses);
        }
        catch (Exception)
        {
            return Results.Problem("There was a problem but its not your fault!", statusCode: 500);
        }
    }
    public static async Task<IResult> GetExpenseByDates([AsParameters] GetExpenseByDateModel dates, 
        [FromServices] IExpenseRepository repo)
    {
        try
        {
            var expenses = await repo.GetManyByDateAsync(dates)
                .ConfigureAwait(false);
            return Results.Ok(expenses);
        }
        catch (Exception)
        {
            return Results.Problem("There was a problem but its not your fault!", statusCode: 500);
        }
    }
    public static async Task<IResult> AddExpense([FromBody] AddExpenseModel model, [FromServices] IExpenseRepository repo)
    {
        try
        {
            var expenseTypes = model.ExpenseTypes.Adapt<List<ExpenseType>>();
            var currency = model.Currency switch
            {
                "USD" => new Currency("USD", "US Dollar", "$"),
                "EUR" => new Currency("EUR", "Euro", "€"),
                "GBP" => new Currency("GBP", "British Pound", "£"),
                _ => new Currency("NGN", "Nigerian Naira", "₦")
            };
            var entity = new Expense(model.Description, new Money(model.Amount, currency),
                model.ExpenseDate);
            foreach (var expenseType in expenseTypes)
            {
                entity.AddExpenseType(expenseType);
            }

            await repo.AddAsync(entity).ConfigureAwait(false);
            return Results.Ok();
        }
        catch (Exception)
        {
            return Results.Problem("There was a problem but its not your fault!", statusCode: 500);
        }
    }
    public static async Task<IResult> UpdateExpense(UpdateExpenseModel model, Guid id, [FromServices] IExpenseRepository repo)
    {
        try
        {
            var expense = model.Adapt<Expense>();
            var updated = await repo.UpdateAsync<ExpenseApiModel>(expense).ConfigureAwait(false);
            return Results.Ok(updated);
        }
        catch (Exception)
        {
            return Results.Problem("There was a problem but its not your fault!", statusCode: 500);
        }
    }
    
    public static async Task<IResult> DeleteExpense(Guid id, IExpenseRepository repo)
    {
        try
        {
            var found = await repo.GetAsync<ExpenseApiModel>(id).ConfigureAwait(false);
            await repo.DeleteAsync<ExpenseApiModel>(new Expense(id)).ConfigureAwait(false);
            return Results.Ok();
        }
        catch (Exception)
        {
            return Results.Problem("There was a problem but its not your fault!", statusCode: 500);
        }
    }
}