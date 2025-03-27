using Fanex.Data.SqlServer;
using NexFB.WorkerService;

var builder = Host.CreateApplicationBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddFanexSql(configuration);
builder.Services.AddApplicationServices(configuration);
builder.Services.AddHttpClient();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
