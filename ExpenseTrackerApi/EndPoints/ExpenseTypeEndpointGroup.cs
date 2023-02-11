using System.Net;
using ExpenseTrackerApi.Abstractions;
using ExpenseTrackerApi.ApiModels;
using ExpenseTrackerApi.DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.EndPoints;

public static class ExpenseTypeEndpointGroup
{
    public static void MapExpenseTypeEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var expenseTypeGroup = endpoints.MapGroup("/api");
        endpoints.MapGet("/expense-types", async ([FromServices]IExpenseTypeRepository repo) =>
        {
            var result = await repo.GetExpenseTypes()
                .ConfigureAwait(false);
            return Results.Ok(result);
        }).Produces(StatusCodes.Status200OK, typeof(IEnumerable<ExpenseTypeApiModel>));

        endpoints.MapGet("/expense-types/{id:guid}", async (Guid id, [FromServices]IExpenseTypeRepository repo) =>
            {
                try
                {
                    var result = await repo.GetExpenseTypeByIdAsync(id)
                        .ConfigureAwait(false);
                    return Results.Ok(result);
                }
                catch (Exception)
                {
                    return Results.Problem("An error occurred but this isn't your doing!");
                }
            }).AddEndpointFilter(async (context, next) =>
            {
                var id = context.GetArgument<Guid>(0);
                if (id == Guid.Empty)
                    return Results.BadRequest("Invalid Id");
                return await next(context).ConfigureAwait(false);
            })
            .Produces(StatusCodes.Status200OK, typeof(ExpenseTypeApiModel))
            .Produces(StatusCodes.Status400BadRequest, typeof(object))
            .Produces(StatusCodes.Status404NotFound, typeof(object))
            .Produces(StatusCodes.Status500InternalServerError, typeof(object));
    }
}