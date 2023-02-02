using ExpenseTrackerApi.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTrackerApi.Data.EntityConfigurations;

public class ExpenseTypeConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasMany(e => e.ExpenseTypes)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(e => e.Description)
            .HasMaxLength(300)
            .IsRequired();
        builder.Property(e => e.ExpenseDate)
            .IsRequired();
        builder.HasQueryFilter(e => e.IsDeleted == false);
        builder.OwnsOne(e => e.Amount, a =>
        {
            a.Property(m => m.Amount)
                .HasPrecision(12,2)
                .IsRequired();
            a.OwnsOne(m => m.Currency, c =>
            {
                c.Property(c => c.Code)
                    .HasMaxLength(3)
                    .IsRequired();
                c.Property(c => c.Name)
                    .HasMaxLength(50)
                    .IsRequired();
                c.Property(c => c.Symbol)
                    .HasMaxLength(5)
                    .IsRequired();
            });
        });
    }
}