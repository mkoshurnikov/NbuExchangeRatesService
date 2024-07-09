# Service Documentation

## Dependencies

- `Microsoft.Extensions.Configuration`\
Provides configuration provider.

- `Newtonsoft.Json`\
Library that give us functionality for serializing to and deserializing objects from JavaScript Object Notation (JSON).

- `Microsoft.Extensions.Logging`\
Provides logging provider.

- `Microsoft.Extensions.Hosting`\
Provides classes that allow us to encapsulate an app's resources and lifetime functionality.

- `Microsoft.Extensions.Hosting.WindowsServices`\
Provides classes that allow us to encapsulate a Windows Service app's resources and lifetime functionality.

## API Endpoints

- `https://bank.gov.ua/NBU_Exchange/exchange?json`\
Provides access to exchange rates data from National Bank of Ukraine in JSON format.

## Code structure overview

### Overview of service classes

- `ConfiguratorService`\
Class is responsible for configuration process and has 2 main methods: `SetConfiguration()` and `Load()`.
`SetConfiguration()` method is responsible for checking config file `config.json` modification state and in case of any changes load revelent data from `config.json` with `Load()` method.

- `DataWriterService`\
Class is responsible for saving fetched exchange rates data to a file and has 1 main method `Save()`.
`Save()` method simply identifies desirable data format for saving exchange rates and call one of 3 methods (`SaveJson`, `SaveXml`, `SaveCsv`) which save data in specific format.

- `ExchangeRatesFetchService`\
Class helps fetch exchange rates from the NBU API and has 1 method: `GetJsonExchangeRates()`.
`GetJsonExchangeRates()` method fetches data from NBU API endpoint `https://bank.gov.ua/NBU_Exchange/exchange?json` and returns in JSON format.

- `ExchangeRatesBackgroundService`\
Class responsible for fetching data from NBU API with certain frequency and saving after that and has 1 method `ExecuteAsync()`.
`ExecuteAsync()` method fetches data from NBU API with certain frequency.

- `Program`
This is start point of application, used for registration required services, building and starting.

### Models overview

- `Currency`
This model is used for mapping data which were fetched from NBU API and.

- `DataFormat`
Model represents enum of possible data formats for storing exchange rates data into file.

### Config
`config.json` file is used to configure service workflow through changing 3 existed variables: `DataFormat`, `FetchFrequency` and 
`FileName`.