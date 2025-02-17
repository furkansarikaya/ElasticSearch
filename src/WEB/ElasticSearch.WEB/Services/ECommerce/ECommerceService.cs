using ElasticSearch.WEB.Repositories.ECommerce;
using ElasticSearch.WEB.ViewModels.ECommerce;

namespace ElasticSearch.WEB.Services.ECommerce;

public class ECommerceService(IECommerceRepository eCommerceRepository) : IECommerceService
{
    public async Task<(List<ECommerceViewModel>?, long totalCount, long pageLinkCount)> SearchAsync(ECommerceSearchViewModel searchModel, int page, int pageSize)
    {
        var (eCommerceList, totalCount) = await eCommerceRepository.SearchAsync(searchModel, page, pageSize);
        if (eCommerceList == null)
        {
            return (null, 0, 0);
        }

        var pageLinkCountCalculate = totalCount % pageSize;
        long pageLinkCount;

        if (pageLinkCountCalculate == 0)
        {
            pageLinkCount = totalCount / pageSize;
        }
        else
        {
            pageLinkCount = (totalCount / pageSize) + 1;

        }

        var eCommerceListViewModel = eCommerceList.Select(x => new ECommerceViewModel()
        {
            Category = string.Join(",", x.Category),
            CustomerFullName = x.CustomerFullName,
            CustomerFirstName = x.CustomerFirstName,
            CustomerLastName = x.CustomerLastName,
            OrderDate = x.OrderDate.ToShortDateString(),
            CustomerGender = x.CustomerGender.ToLower(),
            Id = x.Id,
            OrderId = x.OrderId,
            TaxFulTotalPrice = x.TaxFulTotalPrice
        }).ToList();
        return (eCommerceListViewModel, totalCount, pageLinkCount);
    }
}