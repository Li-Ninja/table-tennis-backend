using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Database.MsSql.TableTennis.Repositories;
using table_tennis_backend.Services;
using Microsoft.EntityFrameworkCore;
using Database.DatabaseConfig;
using System.Text.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");

// Add DbContext registration
var databaseConfig = new DatabaseConfig();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var dbIp = Environment.GetEnvironmentVariable("DB_IP") ?? "";
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";

Console.WriteLine($"DB IP: {dbIp}");

var connectionStringTemplate = builder.Configuration.GetConnectionString("MsSqlConnection") ?? "";

Console.WriteLine($"connectionStringTemplate: {connectionStringTemplate}");

var connectionString = connectionStringTemplate
    .Replace("${DB_IP}", dbIp)
    .Replace("${DB_PASSWORD}", dbPassword);

// 打印替换后的连接字符串进行调试（请在生产环境中移除）
Console.WriteLine($"Final Connection String: {connectionString}");


Console.WriteLine($"databaseConfig.MsSqlConnection: {databaseConfig.MsSqlConnection}");

builder.Services.AddControllers();
builder.Services.AddDbContext<TableTennisContext>(options =>
    options.UseSqlServer(connectionString));

// Add service and repository to the container.
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
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
                                                "https://ttt.redxninja.com",
                                                "https://ttt-admin.redxninja.com",
                                                "https://ttt-api.redxninja.com",
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
