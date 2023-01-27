using Microsoft.EntityFrameworkCore;
using SantaBackEnd;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionString = "Host=localhost:5432;Username=postgres;Password=postgres;Database=postgres";
builder.Services.AddDbContext<postgresContext>(options =>
{
    options.UseNpgsql(connectionString);
    options.EnableSensitiveDataLogging(true);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
