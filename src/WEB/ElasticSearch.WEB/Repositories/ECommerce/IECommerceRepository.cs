using ElasticSearch.WEB.ViewModels.ECommerce;

namespace ElasticSearch.WEB.Repositories.ECommerce;

public interface IECommerceRepository
{
    Task<(List<Models.ECommerce>? list, long count)> SearchAsync(ECommerceSearchViewModel? searchViewModel, int page, int pageSize);
}