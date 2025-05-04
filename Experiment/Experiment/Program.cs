using Experiment.Services;
using HRMWorkflow.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// 任務處理核心服務
builder.Services.AddSingleton<RedisQueueService>();

// 註冊所有 ITaskWorker（純服務類別，不是背景服務）
builder.Services.AddSingleton<ITaskWorker, CheckEmployeeInfoWorker>();
builder.Services.AddSingleton<ITaskWorker, CheckChangeTypeWorker>();
builder.Services.AddSingleton<ITaskWorker, FetchSalaryStructureWorker>();
builder.Services.AddSingleton<ITaskWorker, CalculateAdjustedSalaryWorker>();
builder.Services.AddSingleton<ITaskWorker, CreateApprovalRequestWorker>();
builder.Services.AddSingleton<ITaskWorker, NotifyApproverWorker>();

// 啟動 Dispatcher 背景服務（統一調度任務）
builder.Services.AddHostedService<TaskDispatcher>();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = "localhost:6379"; // 可改成從 appsettings.json 讀取
    return ConnectionMultiplexer.Connect(configuration);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();
