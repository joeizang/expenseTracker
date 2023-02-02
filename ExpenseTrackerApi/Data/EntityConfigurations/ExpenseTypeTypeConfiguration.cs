using ExpenseTrackerApi.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTrackerApi.Data.EntityConfigurations;

public class ExpenseTypeTypeConfiguration : IEntityTypeConfiguration<ExpenseType>
{
    public void Configure(EntityTypeBuilder<ExpenseType> builder)
    {
        builder.HasKey(et => et.Id);
        builder.HasQueryFilter(et => et.IsDeleted == false);
        builder.Property(et => et.Name)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(et => et.Description)
            .HasMaxLength(300)
            .IsRequired(false);
    }
}