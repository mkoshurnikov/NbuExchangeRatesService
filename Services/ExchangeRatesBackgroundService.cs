using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NbuExchangeRatesService.Contracts;
using NbuExchangeRatesService.Models;

namespace NbuExchangeRatesService.Services;

/// <summary>
/// Class responsible for fetching data from NBU API with certain frequency and saving after that.
/// </summary>
public sealed class ExchangeRatesBackgroundService(
    IExchangeRatesFetchService fetchService,
    ILogger<ExchangeRatesBackgroundService> logger,
    IDataWriterService dataWriterService,
    IConfiguratorService config) : BackgroundService
{

    /// <summary>
    /// Fetches data from NBU API with certain frequency.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                config.SetConfiguration();

                List<Currency>? data = await fetchService.GetJsonExchangeRates();

                if (data != null)
                {
                    dataWriterService.SaveData(data);
                }

                await Task.Delay(config.FetchFrequency, stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Cancellation token has been requested. Process will be stopped soon.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);

            Environment.Exit(1);
        }
    }
}
