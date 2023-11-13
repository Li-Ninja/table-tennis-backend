using table_tennis_backend.Database.MsSql.TableTennis.Model;
using Microsoft.EntityFrameworkCore;
using Database.DatabaseConfig;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext registration
var databaseConfig = new DatabaseConfig();
builder.Configuration.GetSection("Database").Bind(databaseConfig);

builder.Services.AddControllers();
builder.Services.AddDbContext<TableTennisContext>(options =>
    options.UseSqlServer(databaseConfig.MsSqlConnection));

// Add services to the container.
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
app.UseAuthorization();
app.MapControllers();

app.Run();
