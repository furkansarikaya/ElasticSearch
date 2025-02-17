using ElasticSearch.WEB.Services.ECommerce;
using ElasticSearch.WEB.ViewModels.ECommerce;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearch.WEB.Controllers;

public class ECommerceController(IECommerceService eCommerceService) : Controller
{
    public async Task<IActionResult> Search([FromQuery] SearchPageViewModel searchPageView)
    {
        var (eCommerceList, totalCount, pageLinkCount) = await eCommerceService.SearchAsync(searchPageView.SearchModel, searchPageView.Page,
            searchPageView.PageSize);
        searchPageView.List = eCommerceList;
        searchPageView.TotalCount = totalCount;
        searchPageView.PageLinkCount = pageLinkCount;
        return View(searchPageView);
    }
}