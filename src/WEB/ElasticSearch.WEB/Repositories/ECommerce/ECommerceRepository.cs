using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticSearch.WEB.ViewModels.ECommerce;

namespace ElasticSearch.WEB.Repositories.ECommerce;

public class ECommerceRepository(ElasticsearchClient elasticsearchClient) : IECommerceRepository
{
    private const string IndexName = "kibana_sample_data_ecommerce";

    public async Task<(List<Models.ECommerce>? list, long count)> SearchAsync(ECommerceSearchViewModel? searchViewModel, int page, int pageSize)
    {
        List<Action<QueryDescriptor<Models.ECommerce>>> listQuery = [];
        
        if (searchViewModel is null)
        {

            listQuery.Add(g=>g.MatchAll(m => {}));
            return await CalculateResultSet(page, pageSize, listQuery);
        }

        if (!string.IsNullOrEmpty(searchViewModel.Category))
        {
            listQuery.Add((q) => q.Match(m => m
                .Field(f => f.Category)
                .Query(searchViewModel.Category)));
        }

        if (!string.IsNullOrEmpty(searchViewModel.CustomerFullName))
        {
            listQuery.Add((q) => q.Match(m => m
                .Field(f => f.CustomerFullName)
                .Query(searchViewModel.CustomerFullName)));
        }

        if (searchViewModel.OrderDateStart.HasValue)
        {
            listQuery.Add(q => q
                .Range(r => r
                    .DateRange(dr => dr
                        .Field(f => f.OrderDate)
                        .Gte(searchViewModel.OrderDateStart.Value))));
        }

        if (searchViewModel.OrderDateEnd.HasValue)
        {
            listQuery.Add(q => q
                .Range(r => r
                    .DateRange(dr => dr
                        .Field(f => f.OrderDate)
                        .Lte(searchViewModel.OrderDateEnd.Value))));
        }

        if (!string.IsNullOrEmpty(searchViewModel.Gender))
        {
            listQuery.Add((q) => q.Term(m => m
                .Field(f => f.CustomerGender)
                .Value(searchViewModel.Gender).CaseInsensitive()));
        }


        if (listQuery.Count == 0)
        {
            listQuery.Add(g => g.MatchAll(m => {}));
        }
        
        return await CalculateResultSet(page, pageSize, listQuery);
    }
    
    
    private async Task<(List<Models.ECommerce>? list, long count)> CalculateResultSet(int page, int pageSize, List<Action<QueryDescriptor<Models.ECommerce>>> listQuery)
    {
        var pageFrom = (page - 1) * pageSize;

        var searchResponse = await elasticsearchClient.SearchAsync<Models.ECommerce>(s => s
            .Index(IndexName)
            .Size(pageSize)
            .From(pageFrom)
            .Query(q => q
                .Bool(b => b
                    .Must(listQuery.ToArray()))));

        if (!searchResponse.IsValidResponse)
            return (null,0);

        foreach (var hit in searchResponse.Hits) hit.Source!.Id = hit.Id!;

        return (list: searchResponse.Documents.ToList(), searchResponse.Total);
    }
}