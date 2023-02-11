using ExpenseTrackerApi.Abstractions;
using ExpenseTrackerApi.Data;
using ExpenseTrackerApi.EndPoints;
using ExpenseTrackerApi.ObjectMappingConfig;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ExpenseTrackerContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("RemotePgsql"));
});
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericDataRepository<>));
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IExpenseTypeRepository, ExpenseTypeRepository>();
builder.Services.RegisterTypeConfiguration();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapExpenseEndpoints();
app.MapExpenseTypeEndpoints();
app.Run();

