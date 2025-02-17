using ElasticSearch.WEB.Repositories.ECommerce;
using ElasticSearch.WEB.Services.ECommerce;

namespace ElasticSearch.WEB.Extensions;

public static class ServiceCollectionExtensions
{
    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IECommerceRepository, ECommerceRepository>();
    }
    
    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IECommerceService, ECommerceService>();
    }
    
    
    public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews();
        services.AddElastic(configuration);
        
        services.AddRepositories();
        services.AddServices();
    }
}