using FluentValidation;

namespace ExpenseTrackerApi.EndPoints;

public static class ExpenseEndpointGroup
{
    public static void MapExpenseEndpoints(this WebApplication app)
    {
        var expenseGroup = app.MapGroup("/expense");

        expenseGroup.MapGet("/", async () =>
        {
            
        })
    }
    
}