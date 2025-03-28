using Fanex.Data.SqlServer;
using NexFB.WorkerService;
using NexFB.WorkerService.Helper;

var builder = Host.CreateApplicationBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddWindowsService();
builder.Services.AddFanexSql(configuration);
builder.Services.AddApplicationServices(configuration);
builder.Services.AddHttpClient();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
LoggingExtension.Config(host.Services.GetRequiredService<ILogger<Worker>>());
host.Run();
