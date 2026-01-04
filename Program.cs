using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Database.MsSql.TableTennis.Repositories;
using table_tennis_backend.Services;
using Microsoft.EntityFrameworkCore;
using Database.DatabaseConfig;
using System.Text.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var environmentName = builder.Environment.EnvironmentName;
// Add DbContext registration
var databaseConfig = new DatabaseConfig();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// NOTE: data from cloud build
var dbIp = Environment.GetEnvironmentVariable("DB_IP") ?? "";
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "";
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";

builder.Configuration.GetSection("Database").Bind(databaseConfig);

var msSqlConnection =
    databaseConfig.MsSqlConnection
    .Replace("${DB_IP}", dbIp)
    .Replace("${DB_PORT}", dbPort)
    .Replace("${DB_USER}", dbUser)
    .Replace("${DB_PASSWORD}", dbPassword);

builder.Services.AddControllers();
builder.Services.AddDbContext<TableTennisContext>(options =>
    options.UseSqlServer(msSqlConnection));

// Add service and repository to the container.
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IScoreService, ScoreService>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IDoublePlayerRepository, DoublePlayerRepository>();
builder.Services.AddScoped<IPlayerScoreHistoryRepository, PlayerScoreHistoryRepository>();
builder.Services.AddScoped<IResultService, ResultService>();
builder.Services.AddScoped<IResultRepository, ResultRepository>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IResultItemService, ResultItemService>();
builder.Services.AddScoped<IResultItemRepository, ResultItemRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName); // 使用完整類型名稱作為 schemaId
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                        policy =>
                        {
                            // TODO 使用環境變數
                            policy.WithOrigins("http://localhost",
                                                "http://localhost:3000",
                                                "http://localhost:9000",
                                                "localhost",
                                                "localhost:9000",
                                                "http://192.168.56.201",
                                                "http://192.168.56.201:9000",
                                                "https://ttt.redxninja.com",
                                                "https://ttt-admin.redxninja.com",
                                                "https://ttt-api.redxninja.com",
                                                "https://ttt.reddo.dev",
                                                "https://ttt-admin.reddo.dev",
                                                "https://ttt-api.reddo.dev",
                                                "https://table-tennis-admin-whvz2vfgsq-de.a.run.app"
                                                )
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                        });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = new JsonCustomNamingPolicy();
});

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation($"Environment: {environmentName}");
logger.LogInformation($"DB IP: {dbIp}");
logger.LogInformation($"DB Port: {dbPort}");
logger.LogInformation($"DB User: {dbUser}");

logger.LogInformation("Begin connection DB.");

// Test database connection
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TableTennisContext>();

    try
    {
        dbContext.Database.OpenConnection();
        logger.LogInformation("Successfully connected to the database.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to connect to the database.");
    }
    finally
    {
        dbContext.Database.CloseConnection();
    }
}

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

public class JsonCustomNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name)) return name;

        var stringBuilder = new StringBuilder();
        bool isNewWord = true;

        foreach (char c in name)
        {
            if (c == '_')
            {
                stringBuilder.Append('_');
                isNewWord = true;
            }
            else if (isNewWord)
            {
                stringBuilder.Append(char.ToLowerInvariant(c));
                isNewWord = false;
            }
            else
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString();
    }
}
