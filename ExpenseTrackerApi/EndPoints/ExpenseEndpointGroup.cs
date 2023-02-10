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

        expenseGroup.MapGet("/expense", async ([FromServices]IRepository<Expense> repo) =>
        {
            try
            {
                var expenses = await repo.GetAllAsync<ExpenseApiModel>().ConfigureAwait(false);
                return Results.Ok(expenses);
            }
            catch (Exception e)
            {
                return Results.Problem("There was a problem. But its not your fault", statusCode: 500);
            }
        }).Produces(StatusCodes.Status200OK, typeof(IEnumerable<ExpenseApiModel>))
            .Produces(StatusCodes.Status500InternalServerError, typeof(object));
        
        expenseGroup.MapGet("/expense/{id:guid}", 
                async (Guid id, [FromServices]IExpenseRepository repo) =>
        {
            try
            {
                var expense = await repo.GetAsync<ExpenseApiModel>(id).ConfigureAwait(false);
                return expense is null ? Results
                    .BadRequest(
                        new
                        {
                            Error = "There seems to be a problem with the id submitted"
                        }) : Results.Ok(expense);
            }
            catch (Exception e)
            {
                return Results.Problem("There was a problem retrieving the expense", statusCode: 500);
            }
        }).AddEndpointFilter(async (context, next) =>
        {
            var id = context.HttpContext.Request.RouteValues["id"] as Guid? ?? Guid.Empty;
            if(id == Guid.Empty)
                return Results.BadRequest("Invalid Id");
            return await next(context).ConfigureAwait(false);
        }).Produces(StatusCodes.Status200OK, typeof(ExpenseApiModel))
            .Produces(StatusCodes.Status400BadRequest, typeof(object))
            .Produces(StatusCodes.Status500InternalServerError, typeof(object));
        
        expenseGroup.MapGet("/expense/{date:datetime}", 
            async (DateTime date, [FromServices]IExpenseRepository repo) =>
        {
            try
            {
                var expenses = await repo.GetByDateAsync<ExpenseApiModel>(date)
                    .ConfigureAwait(false);
                return !expenses.Any() ? Results.BadRequest(
                    new
                    {
                        Error = "There seems to be a problem with the date submitted"
                    }) : Results.Ok(expenses);
            }
            catch (Exception e)
            {
                return Results.Problem("There was a problem but its not your fault!", statusCode: 500);
            }
        }).AddEndpointFilter(async (context, next) =>
        {
            var date = context.HttpContext.Request.RouteValues["date"] as DateTime? ?? DateTime.MinValue;
            if (date == DateTime.MinValue)
                return Results.BadRequest("Invalid Date");
            return await next(context).ConfigureAwait(false);
        }).Produces(StatusCodes.Status200OK, typeof(IEnumerable<ExpenseApiModel>))
            .Produces(StatusCodes.Status400BadRequest, typeof(object))
            .Produces(StatusCodes.Status500InternalServerError, typeof(object));
        
        expenseGroup.MapGet("/expense/description:string",
            async (string description, [FromServices]IExpenseRepository repo) =>
            {
                try
                {
                    var expenses = await repo.GetManyByDescriptionAsync(description)
                        .ConfigureAwait(false);
                    return Results.Ok(expenses);
                }
                catch (Exception e)
                {
                    return Results.Problem("There was a problem but its not your fault!", statusCode: 500);
                }
            }).AddEndpointFilter(async (context, next) =>
            {
                var description = context.HttpContext.Request.RouteValues["description"] as string;
                if (string.IsNullOrWhiteSpace(description))
                    return Results.BadRequest("No expenses found with the description provided");
                return await next(context).ConfigureAwait(false);
            }).Produces(StatusCodes.Status200OK, typeof(IEnumerable<ExpenseApiModel>))
            .Produces(StatusCodes.Status400BadRequest, typeof(object))
            .Produces(StatusCodes.Status500InternalServerError, typeof(object));
        
        expenseGroup.MapGet("/expense/dates", 
            async ([AsParameters]GetExpenseByDateModel dates, [FromServices]IExpenseRepository repo) =>
            {
                try
                {
                    var expenses = await repo.GetManyByDateAsync(dates)
                        .ConfigureAwait(false);
                    return Results.Ok(expenses);
                }
                catch (Exception e)
                {
                    return Results.Problem("There was a problem but its not your fault!", statusCode: 500);
                }
            }).AddEndpointFilter(async (context, next) =>
            {
                var validator = new GetExpenseByDateValidator();
                var model = context.GetArgument<GetExpenseByDateModel>(0);
                var result = await validator.ValidateAsync(model)
                    .ConfigureAwait(false);
                if (!result.IsValid)
                    return Results.BadRequest("One of your dates is in an invalid format");
                return await next(context).ConfigureAwait(false);
            }).Produces(StatusCodes.Status200OK, typeof(IEnumerable<ExpenseApiModel>))
            .Produces(StatusCodes.Status400BadRequest, typeof(object))
            .Produces(StatusCodes.Status500InternalServerError, typeof(object));
        
        expenseGroup.MapPost("/expense", 
                async ([FromBody] AddExpenseModel model, [FromServices]IExpenseRepository repo) =>
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
                var entity = new Expense(model.Description, new Money(model.Amount, currency), model.ExpenseDate);
                foreach (var expenseType in expenseTypes)  
                {
                    entity.AddExpenseType(expenseType);
                }
                await repo.AddAsync(entity).ConfigureAwait(false);
                return Results.Ok();
            }
            catch (Exception e)
            {
                return Results.Problem("There was a problem but its not your fault!", statusCode: 500);
            }
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
        })
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest, typeof(object))
            .Produces(StatusCodes.Status500InternalServerError, typeof(object));
    }
    
}