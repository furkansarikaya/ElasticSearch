using ElasticSearch.WEB.ViewModels.ECommerce;

namespace ElasticSearch.WEB.Services.ECommerce;

public interface IECommerceService
{
    Task<(List<ECommerceViewModel>?, long totalCount, long pageLinkCount)> SearchAsync(ECommerceSearchViewModel searchModel, int page, int pageSize);
}