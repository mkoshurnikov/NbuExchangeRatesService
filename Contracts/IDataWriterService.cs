using NbuExchangeRatesService.Models;

namespace NbuExchangeRatesService.Contracts;

public interface IDataWriterService
{
    void SaveData(List<Currency> data);
}
