using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NbuExchangeRatesService.Models;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using Formatting = Newtonsoft.Json.Formatting;
using NbuExchangeRatesService.Contracts;

namespace NbuExchangeRatesService.Services;

/// <summary>
/// Class is responsible for saving fetched exchange rates data to a file.
/// </summary>
public class DataWriterService(ILogger<DataWriterService> logger, IConfiguratorService config): IDataWriterService
{
    /// <summary>
    /// Folder name where files with exchange rates stores.
    /// </summary>
    private const string FOLDER_NAME = "NbuExchangeRates";

    /// <summary>
    /// Full path to folder where files with exchange rates stores.
    /// </summary>
    private static readonly string FolderFullPath = Path.Combine(AppContext.BaseDirectory, FOLDER_NAME);

    /// <summary>
    /// Full path to file with exchange rates.
    /// </summary>
    private readonly string FileFullPath = Path.Combine(FolderFullPath, config.FileName);

    /// <summary>
    /// Saves data depending on format.
    /// </summary>
    public void SaveData(List<Currency> data)
    {
        if (!Directory.Exists(FolderFullPath))
        {
            Directory.CreateDirectory(FolderFullPath);
        }

        switch (config.DataFormat)
        {
            case DataFormat.json:
                SaveJson(data);
                break;
            case DataFormat.xml:
                SaveXml(data);
                break;
            case DataFormat.csv:
                SaveCsv(data);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(config.DataFormat), config.DataFormat, null);
        }
    }

    /// <summary>
    /// Saves data in JSON format.
    /// </summary>
    private void SaveJson(List<Currency>? data)
    {
        string? formattedData = JsonConvert.SerializeObject(data, Formatting.Indented);
        var jsonFormatPath = FileFullPath + ".json";
        File.WriteAllText(jsonFormatPath, formattedData);
        logger.LogInformation("Data saved to: {path}", jsonFormatPath);
    }

    /// <summary>
    /// Saves data in XML format.
    /// </summary>
    private void SaveXml(List<Currency>? data)
    {
        var xmlFormatPath = FileFullPath + ".xml";
        var xmlSerializer = new XmlSerializer(typeof(List<Currency>));

        using (var writer = XmlWriter.Create(xmlFormatPath, new XmlWriterSettings { Indent = true }))
        {
            xmlSerializer.Serialize(writer, data);
        }
        logger.LogInformation("Data saved to: {path}", xmlFormatPath);
    }

    /// <summary>
    /// Saves data in CSV format.
    /// </summary>
    private void SaveCsv(List<Currency>? data)
    {
        var csvFormatPath = FileFullPath + ".csv";
        var csv = new StringBuilder();
        csv.AppendLine("StartDate,NumericCode,AlphabeticCode,Units,Amount");
        foreach (var d in data!)
            csv.AppendLine($"{d.StartDate},{d.CurrencyCode},{d.CurrencyCodeL},{d.Units},{d.Amount}");

        File.WriteAllText(csvFormatPath, csv.ToString());
        logger.LogInformation("Data saved to: {path}", csvFormatPath);
    }
}