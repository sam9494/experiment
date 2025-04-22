using System.Linq.Expressions;
using System.Reflection;
using Experiment;
using Experiment.Job;
using Hangfire;
using Hangfire.Common;
using Hangfire.Redis.StackExchange;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRecurringJob, ThreeMinutelyJob>();

// 加入 Hangfire 並使用 Redis 作為儲存介面
builder.Services.AddHangfire(configuration =>
{
    configuration.UseRedisStorage("localhost:6379");
});

// 加入 Hangfire Server
builder.Services.AddHangfireServer();


// Register all job classes
var jobTypes = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => typeof(IRecurringJob).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

foreach (var jobType in jobTypes)
{
    builder.Services.AddScoped(jobType);
}
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// 啟用 Hangfire Dashboard
app.UseHangfireDashboard();


// Auto-register recurring jobs based on attribute
// Register jobs from attribute
var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();
foreach (var type in jobTypes)
{
    var attr = type.GetCustomAttribute<RecurringJobAttribute>();
    if (attr == null) continue;

    var jobInstance = ActivatorUtilities.CreateInstance(app.Services, type) as IRecurringJob;
    if (jobInstance == null) continue;

    recurringJobManager.AddOrUpdate(
        type.Name,
        () => jobInstance.ExecuteAsync(),
        attr.Cron
    );
    

}

//Delete Job
recurringJobManager.RemoveIfExists("FiveMinutelyJob");


app.MapGet("/", () => "Hangfire with Redis is running!");
app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}