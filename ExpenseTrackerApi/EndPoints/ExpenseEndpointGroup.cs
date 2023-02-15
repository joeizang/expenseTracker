using ExpenseTrackerApi.Abstractions;
using ExpenseTrackerApi.ApiModels;
using ExpenseTrackerApi.DomainModels;
using ExpenseTrackerApi.DomainModels.ValueObjects;
using ExpenseTrackerApi.EndPoints.EndpointLogic;
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

        expenseGroup.MapGet("/expense", ExpenseEndpointLogic.GetExpenses)
            .Produces(StatusCodes.Status200OK, typeof(IEnumerable<ExpenseApiModel>))
            .Produces(StatusCodes.Status500InternalServerError, typeof(object));

        expenseGroup.MapGet("/expense/{id:guid}", ExpenseEndpointLogic.GetExpenseById)
            .AddEndpointFilter(async (context, next) =>
            {
                var id = context.HttpContext.Request.RouteValues["id"] as Guid? ?? Guid.Empty;
                if (id == Guid.Empty)
                    return Results.BadRequest("Invalid Id");
                return await next(context).ConfigureAwait(false);
            }).Produces(StatusCodes.Status200OK, typeof(ExpenseApiModel))
            .Produces(StatusCodes.Status400BadRequest, typeof(object))
            .Produces(StatusCodes.Status500InternalServerError, typeof(object));

        expenseGroup.MapGet("/expense/{date:datetime}", ExpenseEndpointLogic.GetExpenseByDate)
            .AddEndpointFilter(async (context, next) =>
            {
                var date = context.HttpContext.Request.RouteValues["date"] as DateTime? ?? DateTime.MinValue;
                if (date == DateTime.MinValue)
                    return Results.BadRequest("Invalid Date");
                return await next(context).ConfigureAwait(false);
            }).Produces(StatusCodes.Status200OK, typeof(IEnumerable<ExpenseApiModel>))
            .Produces(StatusCodes.Status400BadRequest, typeof(object))
            .Produces(StatusCodes.Status500InternalServerError, typeof(object));

        expenseGroup.MapGet("/expense/{description:alpha}", ExpenseEndpointLogic.GetExpenseByDescription)
            .AddEndpointFilter(async (context, next) =>
            {
                var description = context.HttpContext.Request.RouteValues["description"] as string;
                if (string.IsNullOrWhiteSpace(description))
                    return Results.BadRequest("No expenses found with the description provided");
                return await next(context).ConfigureAwait(false);
            }).Produces(StatusCodes.Status200OK, typeof(IEnumerable<ExpenseApiModel>))
            .Produces(StatusCodes.Status400BadRequest, typeof(object))
            .Produces(StatusCodes.Status500InternalServerError, typeof(object));

        expenseGroup.MapGet("/expense/dates", ExpenseEndpointLogic.GetExpenseByDates)
            .AddEndpointFilter(async (context, next) =>
            {
                var validator = new GetExpenseByDateValidator();
                var model = context.GetArgument<GetExpenseByDateModel>(0);
                var result = await validator.ValidateAsync(model)
                    .ConfigureAwait(false);
                if (!result.IsValid)
                    return Results.BadRequest(new
                    {
                        Errors = result
                            .Errors
                            .Select(e => e.ErrorMessage)
                    });
                return await next(context).ConfigureAwait(false);
            }).Produces(StatusCodes.Status200OK, typeof(IEnumerable<ExpenseApiModel>))
            .Produces(StatusCodes.Status400BadRequest, typeof(object))
            .Produces(StatusCodes.Status500InternalServerError, typeof(object));

        expenseGroup.MapPost("/expense", ExpenseEndpointLogic.AddExpense)
            .AddEndpointFilter(async (context, next) =>
            {
                var validator = new AddExpenseValidator();
                var target = context.GetArgument<AddExpenseModel>(0);
                var result = await validator.ValidateAsync(target).ConfigureAwait(false);
                if (!result.IsValid)
                {
                    return Results.BadRequest(new
                    {
                        Errors = result
                            .Errors
                            .Select(e => e.ErrorMessage)
                    });
                }

                return await next(context).ConfigureAwait(false);
            })
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest, typeof(object))
            .Produces(StatusCodes.Status500InternalServerError, typeof(object));

        expenseGroup.MapPut("/expense", ExpenseEndpointLogic.UpdateExpense)
            .AddEndpointFilter(async (context, next) =>
            {
                var target = context.GetArgument<UpdateExpenseModel>(0);

                var validator = new UpdateExpenseValidator();
                var result = await validator.ValidateAsync(target).ConfigureAwait(false);
                if (!result.IsValid)
                {
                    return Results.BadRequest(new
                    {
                        Errors = result
                            .Errors
                            .Select(e => e.ErrorMessage)
                    });
                }

                return await next(context).ConfigureAwait(false);
            }).Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest, typeof(object))
            .Produces(StatusCodes.Status500InternalServerError, typeof(object));
        
        expenseGroup.MapDelete("/expense/{id:guid}", ExpenseEndpointLogic.DeleteExpense)
            .AddEndpointFilter(async (context, next) =>
            {
                var id = context.GetArgument<Guid>(0);
                if(id == Guid.Empty)
                    return Results.BadRequest("Invalid Id");
                return await next(context).ConfigureAwait(false);
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest, typeof(object))
            .Produces(StatusCodes.Status500InternalServerError, typeof(object));
    }
    
}