# NBU Exchange Rates Windows Service

Service retrieves exchange rates data from National Bank of Ukraine API.
(https://bank.gov.ua/NBU_Exchange/exchange?json)

## Installation and uninstallation procedures

_All commands should be executed through terminal._

To install windows service:
```bash
sc create "NBU Official Exchange Rates" binpath="~/NbuExchangeRatesService/NbuExchangeRatesService.exe"
```
To start service:
```bash
sc start "NBU Official Exchange Rates"
```

To stop service:
```bash
sc stop "NBU Official Exchange Rates"
```
To delete service:
```bash
sc delete "NBU Official Exchange Rates"
```

## Configuration options

In _NBU Official Exchange Rates_ service we have `Configurator` class that retrieves values from configuration file `config.json`, so to customize service workflow we can easily change values of these variables.

## Dependencies

  - `Microsoft.Extensions.Configuration`
  - `Newtonsoft.Json`
  - `Microsoft.Extensions.Logging`
  - `Microsoft.Extensions.Hosting`
  - `Microsoft.Extensions.Hosting.WindowsServices`

## Control codes

To customize service workflow we can go to file `config.json`, path: `~/NbuExchangeRatesService/bin/Debug/net8.0/config.json`:

```json
{
  "DataFormat": "",
  "FetchFrequency": 0,
  "FileName": ""
}
```

Configuration file include 3 variables: `DataFormat`, `FetchFrequency` and `FileName`.
- Set `DataFormat` variable to choose desirable data format for saving retrieved exchange rates from NBU API. Possible formats: `json`, `xml`, `csv`. If not set, default format: `json`.
- Set `FetchFrequency` variable to choose delay between calls to NBU API in ms. If not set, default value: `10000`ms or `10`sec.
- Set `FileName` variable to choose desirable file name. If not set, default name: `ExchangeRates`.

## Troubleshooting tips

### Why service wasn't removed after bash command?

- Solution: execute bash `stop` command before deletion.

### Config error

- Solution: make sure to set only validate data. Replace config file content for new one from section below or from `config.json` which located in base path: `~/NbuExchangeRatesService/config.json`.

### Why config changes doesn't work?

- Solution: make sure to use config from `~NbuExchangeRatesService/bin/Debug/net8.0` folder. Changes in config from the root path doesn't apply when app is running.

### Why an error occured while fetching data from API?

- Solution: check your internet connection.

### Why access is denied while perfoming operations in terminal?

- Solution: run terminal as administrator.