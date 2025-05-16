using Fanex.Data.SqlServer;
using Fanex.Logging.Sentry.RavenClient;
using NexFB.WorkerService;
using NexFB.WorkerService.Common;
using NexFB.WorkerService.Helper;
using NexFB.WorkerService.Middlewares;
using Serilog;
using Serilog.Events;

var builder = Host.CreateApplicationBuilder(args);
var configuration = builder.Configuration;

var baseDir = AppContext.BaseDirectory;
var logDir = Path.Combine(baseDir, "logs");
Directory.CreateDirectory(logDir);

var logPath = Path.Combine(logDir, "log.txt");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft.Extensions.Http", LogEventLevel.Warning)
    .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
    .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

builder.Services.AddWindowsService();
builder.Services.AddFanexSql(configuration);
builder.Services.AddFanexLogSentry(configuration);
builder.Services.AddApplicationServices(configuration);
builder.Services.AddHttpClient();

builder.Services.AddHostedService<Worker>();

builder.Services.AddTransient<LoggingRequestMiddleware>();

builder.Services.AddHttpClient(Constants.FnBClient)
    .AddHttpMessageHandler<LoggingRequestMiddleware>();

var host = builder.Build();

LoggingExtension.Config(host.Services.GetRequiredService<ILogger<Worker>>());
await host.RunAsync();