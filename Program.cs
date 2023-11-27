using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Database.MsSql.TableTennis.Repositories;
using table_tennis_backend.Services;
using Microsoft.EntityFrameworkCore;
using Database.DatabaseConfig;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext registration
var databaseConfig = new DatabaseConfig();
var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Configuration.GetSection("Database").Bind(databaseConfig);

builder.Services.AddControllers();
builder.Services.AddDbContext<TableTennisContext>(options =>
    options.UseSqlServer(databaseConfig.MsSqlConnection));

// Add service and repository to the container.
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                        policy =>
                        {
                            // TODO 使用環境變數
                            policy.WithOrigins("http://localhost",
                                                "http://localhost:9000",
                                                "localhost",
                                                "localhost:9000",
                                                "https://ttt.groninja.com"
                                                );
                          });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
