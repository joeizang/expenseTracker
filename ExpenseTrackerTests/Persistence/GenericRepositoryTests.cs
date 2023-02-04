using ExpenseTrackerApi.Abstractions;
using ExpenseTrackerApi.ApiModels;
using ExpenseTrackerApi.Data;
using ExpenseTrackerApi.DomainModels;
using ExpenseTrackerApi.DomainModels.ValueObjects;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerTests.Persistence;

public class GenericRepositoryTests
{
    [Fact]
    public async Task CanAddExpenseType()
    {
        var options = new DbContextOptionsBuilder<ExpenseTrackerContext>()
            .UseInMemoryDatabase(databaseName: "ExpenseTracker")
            .Options;

        await using var context = new ExpenseTrackerContext(options);
        var repository = new GenericDataRepository<ExpenseType>(context);

        var expenseType = new ExpenseType
        {
            Name = "Groceries",
            Description = "Groceries from Sundays Shop"
        };

        await repository.AddAsync(expenseType);

        var expenseTypeFromDb = await repository.GetAsync<ExpenseTypeApiModel>(expenseType.Id);

        Assert.IsType<ExpenseTypeApiModel>(expenseTypeFromDb);
        Assert.Equal(expenseTypeFromDb.Name, expenseType.Name);
    }
    
    [Fact]
    public async Task CanAddExpense()
    {
        var options = new DbContextOptionsBuilder<ExpenseTrackerContext>()
            .UseInMemoryDatabase(databaseName: "ExpenseTracker")
            .Options;

        await using var context = new ExpenseTrackerContext(options);
        var repository = new GenericDataRepository<Expense>(context);

        var expense = new Expense
        {
            Description = "Groceries from Sundays Shop",
            Amount = new Money(10000, new Currency("NGN", "Naira", "₦")),
            ExpenseDate = DateTime.Now,
        };
        expense.AddExpenseType(new ExpenseType
            {
                Name = "Groceries",
                Description = "Groceries from Sundays Shop"
            });

        await repository.AddAsync(expense);

        var expenseFromDb = await repository.GetAsync<ExpenseApiModel>(expense.Id);

        Assert.IsType<ExpenseApiModel>(expenseFromDb);
        Assert.Equal(expenseFromDb.Description, expense.Description);
    }

    [Fact]
    public async Task CanAddExpenseAndPayWithDollars()
    {
            var options = new DbContextOptionsBuilder<ExpenseTrackerContext>()
            .UseInMemoryDatabase(databaseName: "ExpenseTracker")
            .Options;

        await using var context = new ExpenseTrackerContext(options);
        var repository = new GenericDataRepository<Expense>(context);

        var expense = new Expense
        {
            Description = "AlgoExpert.io Subscription",
            Amount = new Money(100, new Currency("USD", "Dollars", "$")),
            ExpenseDate = DateTime.Now,
        };
        expense.AddExpenseType(new ExpenseType
        {
            Name = "Subscription",
            Description = "Online Subscription"
        });

        await repository.AddAsync(expense);

        var expenseFromDb = await repository.GetAsync<ExpenseApiModel>(expense.Id);

        Assert.IsType<ExpenseApiModel>(expenseFromDb);
        Assert.Equal("USD", expenseFromDb.Amount.Currency.Code);
        Assert.Equal(expenseFromDb.Description, expense.Description);
    }

    [Fact]
    public async Task CanUpdateExpenseType()
    {
        var options = new DbContextOptionsBuilder<ExpenseTrackerContext>()
            .UseInMemoryDatabase(databaseName: "ExpenseTracker")
            .Options;

        await using var context = new ExpenseTrackerContext(options);
        var repository = new GenericDataRepository<ExpenseType>(context);

        var expenseType = new ExpenseType
        {
            Name = "Groceries",
            Description = "Groceries from Sundays Shop"
        };

        await repository.AddAsync(expenseType);

        var expenseTypeFromDb = await repository.GetAsync<ExpenseTypeApiModel>(expenseType.Id);

        Assert.IsType<ExpenseTypeApiModel>(expenseTypeFromDb);
        Assert.Equal(expenseTypeFromDb.Name, expenseType.Name);

        expenseTypeFromDb.Name = "Groceries";
        expenseTypeFromDb.Description = "Groceries for a household";

        await repository.UpdateAsync<ExpenseTypeApiModel>(expenseTypeFromDb.Adapt<ExpenseType>());

        var updatedExpenseTypeFromDb = await repository.GetAsync<ExpenseTypeApiModel>(expenseType.Id);

        Assert.IsType<ExpenseTypeApiModel>(updatedExpenseTypeFromDb);
        Assert.Equal(updatedExpenseTypeFromDb.Name, expenseTypeFromDb.Name);
        Assert.NotEqual(updatedExpenseTypeFromDb.Description, expenseTypeFromDb.Description);
    }

    [Fact]
    public async Task CanGetExpensesByDate()
    {
            var options = new DbContextOptionsBuilder<ExpenseTrackerContext>()
            .UseInMemoryDatabase(databaseName: "ExpenseTracker")
            .Options;

        await using var context = new ExpenseTrackerContext(options);
        var repository = new GenericDataRepository<Expense>(context);

        var expense = new Expense
        {
            Description = "Groceries from Sundays Shop",
            Amount = new Money(10000, new Currency("NGN", "Naira", "₦")),
            ExpenseDate = DateTime.Now.AddDays(-15),
        };
        expense.AddExpenseType(new ExpenseType
        {
            Name = "Groceries",
            Description = "Groceries from Sundays Shop"
        });
        var expense1 = new Expense
        {
            Description = "AlgoExpert.io Subscription",
            Amount = new Money(100, new Currency("USD", "Dollars", "$")),
            ExpenseDate = DateTime.Now.AddMonths(-1),
        };
        expense1.AddExpenseType(new ExpenseType
        {
            Name = "Subscription",
            Description = "Online Subscription"
        });

        await repository.AddAsync(expense);
        await repository.AddAsync(expense1);
        var expenseFromDb = await repository
            .GetByDateAsync<ExpenseApiModel>(expense.CreatedAt);

        Assert.IsAssignableFrom<IEnumerable<ExpenseApiModel>>(expenseFromDb);
        Assert.Single(expenseFromDb);
    }

    [Fact]
    public async Task DeleteExpenseType()
    {
        var options = new DbContextOptionsBuilder<ExpenseTrackerContext>()
            .UseInMemoryDatabase(databaseName: "ExpenseTracker")
            .Options;

        await using var context = new ExpenseTrackerContext(options);
        var repository = new GenericDataRepository<ExpenseType>(context);

        var expenseType = new ExpenseType
        {
            Name = "Groceries",
            Description = "Groceries from Sundays Shop"
        };

        await repository.AddAsync(expenseType);

        var expenseTypeFromDb = await repository.GetAsync<ExpenseTypeApiModel>(expenseType.Id);

        Assert.IsType<ExpenseTypeApiModel>(expenseTypeFromDb);
        Assert.Equal(expenseTypeFromDb.Name, expenseType.Name);

        var deletedExpenseTypeFromDb = await repository.DeleteAsync<ExpenseApiModel>(
            expenseTypeFromDb.Adapt<ExpenseType>());
        Assert.True(deletedExpenseTypeFromDb);
    }
}