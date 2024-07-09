using Microsoft.Extensions.Logging;
using NbuExchangeRatesService.Contracts;
using NbuExchangeRatesService.Models;
using System.Net.Http.Json;

namespace NbuExchangeRatesService.Services;

/// <summary>
/// Class helps fetch exchange rates from the NBU API.
/// </summary>
public sealed class ExchangeRatesFetchService(ILogger<ExchangeRatesFetchService> logger): IExchangeRatesFetchService
{
    /// <summary>
    /// NBU API URL for fetching JSON exchange rates.
    /// </summary>
    private const string JSON_API_URL = "https://bank.gov.ua/NBU_Exchange/exchange?json";

    /// <summary>
    /// Fetches exchange rates from the NBU API in JSON format.
    /// </summary>
    /// <returns>String of JSON data, otherwise null if an error occurs.</returns>
    public async Task<List<Currency>?> GetJsonExchangeRates()
    {
        logger.LogInformation($"Trying to fetch data from: {JSON_API_URL}.");
        try
        {
            using (HttpClient httpClient = new HttpClient())
            {
                List<Currency>? data = await httpClient.GetFromJsonAsync<List<Currency>>(JSON_API_URL);
                logger.LogInformation("Data successfully fetched from: {url}. Time: {time}", JSON_API_URL, DateTime.Now.ToString());
                return data;
            }
        }
        catch (Exception e)
        {
            logger.LogError("Error occured, when fetching data from: {url}.\nError: {error}", JSON_API_URL, e);
            return null;
        }
    }
}