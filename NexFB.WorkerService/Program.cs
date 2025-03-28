using Fanex.Data.SqlServer;
using NexFB.WorkerService;
using NexFB.WorkerService.Helper;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);
var configuration = builder.Configuration;

var baseDir = AppContext.BaseDirectory;
var logDir = Path.Combine(baseDir, "logs");
Directory.CreateDirectory(logDir); 

var logPath = Path.Combine(logDir, "log.txt");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

builder.Services.AddWindowsService();
builder.Services.AddFanexSql(configuration);
builder.Services.AddApplicationServices(configuration);
builder.Services.AddHttpClient();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
LoggingExtension.Config(host.Services.GetRequiredService<ILogger<Worker>>());
host.Run();
