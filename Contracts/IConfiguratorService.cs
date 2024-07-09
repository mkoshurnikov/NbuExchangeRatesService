using NbuExchangeRatesService.Models;

namespace NbuExchangeRatesService.Contracts;

public interface IConfiguratorService
{
    void SetConfiguration();

    int FetchFrequency { get; }

    DataFormat DataFormat { get; }

    string FileName { get; }
}
