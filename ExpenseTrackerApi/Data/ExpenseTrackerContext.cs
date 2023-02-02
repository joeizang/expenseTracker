using ExpenseTrackerApi.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Data;

public class ExpenseTrackerContext : DbContext
{
    public ExpenseTrackerContext(DbContextOptions<ExpenseTrackerContext> options) : base(options)
    {
    }

    public DbSet<Expense> Expenses { get; set; } = default!;
    
    public DbSet<ExpenseType> ExpenseTypes { get; set; } = default!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpenseTrackerContext).Assembly);
    }
    
}