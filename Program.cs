using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using NbuExchangeRatesService.Services;
using NbuExchangeRatesService.Contracts;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService(options => options.ServiceName = "NBU Official Exchange Rates");

LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(builder.Services);

builder.Services.AddSingleton<IConfiguratorService, ConfiguratorService>(provider => new ConfiguratorService(
    provider.GetRequiredService<IConfiguration>(),
    provider.GetRequiredService<ILogger<ConfiguratorService>>()));

builder.Services.AddSingleton<IExchangeRatesFetchService, ExchangeRatesFetchService>(provider => new ExchangeRatesFetchService(
    provider.GetRequiredService<ILogger<ExchangeRatesFetchService>>()));

builder.Services.AddSingleton<IDataWriterService, DataWriterService>(provider => new DataWriterService(
    provider.GetRequiredService<ILogger<DataWriterService>>(), 
    provider.GetRequiredService<IConfiguratorService>()));

builder.Services.AddHostedService<ExchangeRatesBackgroundService>();

var host = builder.Build();
host.Run();