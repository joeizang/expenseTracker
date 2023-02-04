using ExpenseTrackerApi.Abstractions;
using ExpenseTrackerApi.ApiModels;
using ExpenseTrackerApi.Data;
using ExpenseTrackerApi.DomainModels;
using ExpenseTrackerApi.DomainModels.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerTests.Persistence;

public class ExpenseRepositoryTests
{
    [Fact]
    public async Task GetManyExpensesByDescription()
    {
        var options = new DbContextOptionsBuilder<ExpenseTrackerContext>()
            .UseInMemoryDatabase(databaseName: "ExpenseTracker")
            .Options;
        var context = new ExpenseTrackerContext(options);
        var repository = new ExpenseRepository(context);
        var expense = new Expense
        {
            Description = "Test Expense",
            Amount = new Money(1000, new Currency("NGN", "Naira", "₦")),
            ExpenseDate = DateTime.Now,
        };
        expense.AddExpenseType(new ExpenseType
        {
            Name = "Test Expense Type",
            Description = "Test Expense Type Description"
        });
        var expense1 = new Expense
        {
            Description = "Test Expense",
            Amount = new Money(2000, new Currency("NGN", "Naira", "₦")),
            ExpenseDate = DateTime.Now,
        };
        expense1.AddExpenseType(new ExpenseType
        {
            Name = "Test Expense Type",
            Description = "Test Expense Type Description"
        });
        await repository.AddAsync(expense);
        await repository.AddAsync(expense1);
        
        var result = await repository.GetManyByDescriptionAsync("Test Expense");
        Assert.IsAssignableFrom<IEnumerable<ExpenseApiModel>>(result);
        Assert.NotEqual(0, result.Count());
    }
    
    [Fact]
    public async Task GetManyExpensesByDate()
    {
        var options = new DbContextOptionsBuilder<ExpenseTrackerContext>()
            .UseInMemoryDatabase(databaseName: "ExpenseTracker")
            .Options;
        var context = new ExpenseTrackerContext(options);
        var repository = new ExpenseRepository(context);
        var expense = new Expense
        {
            Description = "Test Expense",
            Amount = new Money(1000, new Currency("NGN", "Naira", "₦")),
            ExpenseDate = DateTime.Now,
        };
        expense.AddExpenseType(new ExpenseType
        {
            Name = "Test Expense Type",
            Description = "Test Expense Type Description"
        });
        var expense1 = new Expense
        {
            Description = "Test Expense",
            Amount = new Money(2000, new Currency("NGN", "Naira", "₦")),
            ExpenseDate = DateTime.Now,
        };
        expense1.AddExpenseType(new ExpenseType
        {
            Name = "Test Expense Type",
            Description = "Test Expense Type Description"
        });
        await repository.AddAsync(expense);
        await repository.AddAsync(expense1);
        
        var result = await repository.GetManyByDateAsync(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));
        Assert.IsAssignableFrom<IEnumerable<ExpenseApiModel>>(result);
        Assert.NotEqual(0, result.Count());
    }
}
