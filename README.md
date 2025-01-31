# ElasticSearch API

This project is an ASP.NET Core Web API application that interacts with ElasticSearch.

## Requirements

- .NET 9.0 SDK
- ElasticSearch 7.17.5

## Installation

1. Clone this repository:
    ```sh
    git clone https://github.com/username/repositoryname.git
    cd repositoryname/src/API/ElasticSearch.API
    ```

2. Install the required NuGet packages:
    ```sh
    dotnet restore
    ```

3. Update the ElasticSearch connection information in the `appsettings.Development.json` file:
    ```json
    {
      "Elastic": {
        "Url": "http://localhost:9200",
        "Username": "elastic",
        "Password": "changeme"
      }
    }
    ```

## Running

You can run the application with the following command:
```sh
dotnet run