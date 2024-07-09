using Newtonsoft.Json;
using System.Xml.Serialization;

namespace NbuExchangeRatesService.Models;

/// <summary>
/// Model represents a currency that retrieved from the NBU (National Bank of Ukraine) API.
/// </summary>
public class Currency
{
    /// <summary>
    /// Currency exchange start date.
    /// </summary>
    public string? StartDate { get; set; }

    [JsonIgnore]
    [XmlIgnore]
    public int TimeSign { get; set; }

    /// <summary>
    /// Numeric country currency code.
    /// </summary>
    [JsonProperty("NumericCode")] public int CurrencyCode { get; set; }

    /// <summary>
    /// Alphabetic country currency code.
    /// </summary>
    [JsonProperty("AlphabeticCode")] public string? CurrencyCodeL { get; set; }

    /// <summary>
    /// Currency units.
    /// </summary>
    public int Units { get; set; }

    /// <summary>
    /// Currency exchange amount(UAH).
    /// </summary>
    public float Amount { get; set; }
}