using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace ElasticSearch.WEB.Extensions;

public static class ElasticSearchExtensions
{
    public static void AddElastic(this IServiceCollection services, IConfiguration configuration)
    {
        // Read the Elastic URL from the configuration
        var elasticUrl = configuration.GetSection("Elastic")["Url"];
        var userName = configuration.GetSection("Elastic")["Username"];
        var password = configuration.GetSection("Elastic")["Password"];
        if (string.IsNullOrEmpty(elasticUrl))
            throw new ArgumentException("Elastic URL is not configured properly in appsettings.json.");

        var settings = new ElasticsearchClientSettings(new Uri(elasticUrl));
        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
        {
            settings.Authentication(new BasicAuthentication(userName, password));
        }

        var client = new ElasticsearchClient(settings);
        services.AddSingleton(client);
    }
}