using Elasticsearch.Net;
using Nest;

namespace ElasticSearch.API.Extensions;

public static class ElasticSearchExtensions
{
    public static void AddElastic(this IServiceCollection services, IConfiguration configuration)
    {
        // Read the Elastic URL from the configuration
        var elasticUrl = configuration.GetSection("Elastic")["Url"];
        if (string.IsNullOrEmpty(elasticUrl))
            throw new ArgumentException("Elastic URL is not configured properly in appsettings.json.");

        // Set up connection pool and client settings
        var pool = new SingleNodeConnectionPool(new Uri(elasticUrl));
        var settings = new ConnectionSettings(pool);

        // Create and register ElasticClient as a singleton service
        var client = new ElasticClient(settings);
        services.AddSingleton(client);
    }
}