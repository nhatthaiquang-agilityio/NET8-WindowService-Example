using Serilog;
using Serilog.Exceptions;
using WorkerService;

var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService();
builder.Logging.ClearProviders().AddSerilog(
    new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .Enrich.WithExceptionDetails()
        .Enrich.WithProperty("Environment", configuration.GetValue<string>("Environment") ?? "Local")
        .CreateLogger()
);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
