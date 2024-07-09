using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NbuExchangeRatesService.Models;
using NbuExchangeRatesService.Contracts;

namespace NbuExchangeRatesService.Services;

/// <summary>
/// Class responsible for configuration process.
/// </summary>
public class ConfiguratorService: IConfiguratorService
{
    /// <summary>
    /// Default fetch frequency(ms).
    /// </summary>
    private const int DEFAULT_FREQUENCY = 10000;

    /// <summary>
    /// Default file name.
    /// </summary>
    private const string DEFAULT_FILENAME = "ExchangeRates";

    /// <summary>
    /// Config file name.
    /// </summary>
    private const string CONFIG_FILE_NAME = "config.json";

    /// <summary>
    /// Config path location.
    /// </summary>
    private static readonly string ConfigFullPath = Path.Combine(AppContext.BaseDirectory, CONFIG_FILE_NAME);

    /// <summary>
    /// Logger instance for logging messages.
    /// </summary>
    private readonly ILogger<ConfiguratorService> _logger;

    /// <summary>
    /// Config instance.
    /// </summary>
    private IConfiguration _configuration;

    /// <summary>
    /// Fetch frequency(ms).
    /// </summary>
    public int FetchFrequency { get; private set; }

    /// <summary>
    /// File format for saving retrieved exchange rates.
    /// </summary>
    public DataFormat DataFormat { get; private set; }

    /// <summary>
    /// File name for saving exchange rates.
    /// </summary>
    public string FileName { get; private set; } = string.Empty;

    /// <summary>
    /// Timestamp of last changes in config file.
    /// </summary>
    private DateTime TimeStamp { get; set; }

    /// summary>
    /// Initializes new instance and set default configuration.
    /// </summary>
    public ConfiguratorService(IConfiguration configuration, ILogger<ConfiguratorService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        Load();
    }

    /// <summary>
    /// Set configuration before operations in case of changes.
    /// </summary>
    public void SetConfiguration()
    {
        _logger.LogInformation("Check if config is up-to-date:");
        DateTime currentTimeStamp = GetTimeStamp();
        if (currentTimeStamp != TimeStamp)
        {
            _logger.LogInformation("Some changes was tracked. Config requires reload.");
            Load();
        }
        else
        {
            _logger.LogInformation("Config is up-to-date, no actions required.");
        }
    }

    /// <summary>
    /// Loads configuration from config.json file.
    /// </summary>
    private void Load()
    {
        _logger.LogInformation("Loading configuration...");
        _configuration = BuildConfig();
        TimeStamp = GetTimeStamp();
        
        FetchFrequency = string.IsNullOrEmpty(_configuration["FetchFrequency"]) || int.Parse(_configuration["FetchFrequency"]!) == 0
            ? DEFAULT_FREQUENCY : int.Parse(_configuration["FetchFrequency"]!);

        Enum.TryParse(_configuration["DataFormat"], out DataFormat format);
        DataFormat = format;

        FileName = string.IsNullOrEmpty(_configuration["FileName"])
            ? DEFAULT_FILENAME : _configuration["DataName"]!;

        _logger.LogInformation("Config has been loaded, last modified: {time}.", TimeStamp);
    }

    /// <summary>
    /// Gets date of last modification.
    /// </summary>
    /// <returns>Date of last modification.</returns>
    private DateTime GetTimeStamp()
    {
        FileInfo fileInfo = new FileInfo(ConfigFullPath);
        return fileInfo.LastWriteTime;
    }

    /// <summary>
    /// Builds new config.
    /// </summary>
    /// <returns>New config.</returns>
    private static IConfiguration BuildConfig()
    {
        return new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                    .AddEnvironmentVariables()
                    .AddJsonFile("config.json")
                    .Build();
    }
}