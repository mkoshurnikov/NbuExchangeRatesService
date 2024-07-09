using NbuExchangeRatesService.Models;

namespace NbuExchangeRatesService.Contracts;

public interface IExchangeRatesFetchService
{
    Task<List<Currency>?> GetJsonExchangeRates();
}