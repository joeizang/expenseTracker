using ExpenseTrackerApi.ApiModels;
using ExpenseTrackerApi.DomainModels;
using ExpenseTrackerApi.DomainModels.ValueObjects;
using Mapster;

namespace ExpenseTrackerApi.ObjectMappingConfig;

public static class RegisterMapsterTypeConfigs
{
    public static void RegisterTypeConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<AddExpenseModel, Expense>.NewConfig()
            .Map(dest => dest.Amount, 
                src => new Money(src.Amount, 
                    new Currency("NGN", "Naira", "₦")));
        TypeAdapterConfig<ExpenseTypeApiModel, ExpenseType>.NewConfig()
            .Map(dest => dest.Id, src => src.ExpenseTypeId)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description);

        TypeAdapterConfig.GlobalSettings.Scan(typeof(RegisterMapsterTypeConfigs).Assembly);
    }
}