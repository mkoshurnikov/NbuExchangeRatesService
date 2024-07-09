namespace NbuExchangeRatesService.Models;

/// <summary>
/// Model represents posible data formats for saving exchange rates.
/// </summary>
public enum DataFormat
{
    /// <summary>
    /// JSON format.
    /// </summary>
    json,

    /// <summary>
    /// XML format.
    /// </summary>
    xml,

    /// <summary>
    /// CSV format.
    /// </summary>
    csv
}