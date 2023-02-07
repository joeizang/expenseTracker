using ExpenseTrackerApi.Abstractions;
using ExpenseTrackerApi.ApiModels;
using ExpenseTrackerApi.DomainModels;
using ExpenseTrackerApi.DomainModels.ValueObjects;
using ExpenseTrackerApi.Validators.Expense;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;

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
        
        expenseGroup.MapPost("/expense", async ([FromBody] AddExpenseModel model, 
            IRepository<Expense> repo) =>
        {
            var entity = model.Adapt<Expense>();
            var expenseTypes = model.ExpenseTypes.Adapt<List<ExpenseType>>();
            var currency = model.Currency switch
            {
                "USD" => new Currency("USD", "US Dollar", "$"),
                "EUR" => new Currency("EUR", "Euro", "€"),
                "GBP" => new Currency("GBP", "British Pound", "£"),
                _ => new Currency("NGN", "Nigerian Naira", "₦")
            };
            entity.Amount = new Money(model.Amount, currency);
            foreach (var expenseType in expenseTypes)  
            {
                entity.AddExpenseType(expenseType);
            }
            await repo.AddAsync(entity).ConfigureAwait(false);
            return Results.Ok();
        }).AddEndpointFilter(async(context, next) =>
        {
            var validator = new AddExpenseValidator();
            var target = context.GetArgument<AddExpenseModel>(0);
            var result = await validator.ValidateAsync(target).ConfigureAwait(false);
            if (!result.IsValid)
            {
                return Results.BadRequest(result.Errors);
            }
            return await next(context).ConfigureAwait(false);
        });
    }
    
}