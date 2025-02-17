using System.Collections.Immutable;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticSearch.API.Models;

namespace ElasticSearch.API.Repositories;

public class ECommerceRepository(ElasticsearchClient client)
{
    private const string IndexName = "kibana_sample_data_ecommerce";

    public async Task<ImmutableList<ECommerce>> TermLevelQueryAsync(string customerFirstName)
    {
        //1. way
        // var result = await client.SearchAsync<ECommerce>(s =>
        //     s.Index(IndexName)
        //         .Query(q =>
        //             q.Term(t =>
        //                 t.Field("customer_first_name.keyword"!).Value(customerFirstName))));

        //2. way
        // var result = await client.SearchAsync<ECommerce>(s =>
        //     s.Index(IndexName)
        //         .Query(q =>
        //             q.Term(t =>
        //                 t.Field(f => f.CustomerFirstName.Suffix("keyword")).Value(customerFirstName))));

        //3. way
        var termQuery = new TermQuery("customer_first_name.keyword") { Value = customerFirstName, CaseInsensitive = true };
        var result = await client.SearchAsync<ECommerce>(s =>
            s.Index(IndexName)
                .Query(termQuery));

        foreach (var hit in result.Hits) hit!.Source!.Id = hit!.Id!;
        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> TermsQueryAsync(List<string> customerFirstNameList)
    {
        List<FieldValue> terms = [];
        customerFirstNameList.ForEach(x => { terms.Add(x); });
        //1.way
        // var termsQuery = new TermsQuery()
        // {
        //     Field = "customer_first_name.keyword",
        //     Terms = new TermsQueryField(terms.AsReadOnly())
        // };
        //
        // var result = await client.SearchAsync<ECommerce>(s => s.Index(IndexName).Query(termsQuery));

        // 2. way
        var result = await client.SearchAsync<ECommerce>(s => s.Index(IndexName)
            .Size(100)
            .Query(q => q
                .Terms(t => t
                    .Field(f => f.CustomerFirstName
                        .Suffix("keyword"))
                    .Terms(new TermsQueryField(terms.AsReadOnly())))));

        foreach (var hit in result.Hits) hit!.Source!.Id = hit!.Id!;
        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> PrefixQueryAsync(string customerFullName)
    {
        var result = await client.SearchAsync<ECommerce>(s =>
            s.Index(IndexName)
                .Query(q =>
                    q.Prefix(p =>
                        p.Field(f => f.CustomerFullName.Suffix("keyword")).Value(customerFullName))));

        foreach (var hit in result.Hits) hit!.Source!.Id = hit!.Id!;
        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> RangeQueryAsync(double fromPrice, double toPrice)
    {
        var result = await client.SearchAsync<ECommerce>(s =>
            s.Index(IndexName)
                .Query(q =>
                    q.Range(r =>
                        r.NumberRange(nr =>
                            nr.Field(f => f.TaxFulTotalPrice)
                                .Gte(fromPrice)
                                .Lte(toPrice)))));

        foreach (var hit in result.Hits) hit!.Source!.Id = hit!.Id!;
        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>> MatchAllQueryAsync(int size)
    {
        var result = await client.SearchAsync<ECommerce>(s =>
            s.Index(IndexName)
                .Size(size)
                .Query(q => q.MatchAll(m => {})));

        foreach (var hit in result.Hits) hit!.Source!.Id = hit!.Id!;
        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>?> PaginationQueryAsync(int page, int pageSize)
    {
        // page=1, pageSize=10 =>  1-10
        // page=2 , pageSize=10=> 11-20
        // page=3, pageSize=10 => 21-30

        var pageFrom = (page - 1) * pageSize;
        var result = await client.SearchAsync<ECommerce>(s =>
            s.Index(IndexName)
                .Size(pageSize)
                .From(pageFrom)
                .Query(q => q.MatchAll(m => {})));
        if (!result.IsValidResponse)
            return null;
        foreach (var hit in result.Hits) hit!.Source!.Id = hit!.Id!;
        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>?> WildcardQueryAsync(string customerFullName)
    {
        var result = await client.SearchAsync<ECommerce>(s =>
            s.Index(IndexName)
                .Query(q =>
                    q.Wildcard(w =>
                        w.Field(f => f.CustomerFullName.Suffix("keyword")).Wildcard(customerFullName))));
        if (!result.IsValidResponse)
            return null;
        foreach (var hit in result.Hits) hit!.Source!.Id = hit!.Id!;
        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>?> FuzzyQueryAsync(string customerName)
    {
        var result = await client.SearchAsync<ECommerce>(s =>
            s.Index(IndexName)
                .Query(q =>
                    q.Fuzzy(fu =>
                        fu.Field(f => f.CustomerFirstName.Suffix("keyword")).Value(customerName)
                            .Fuzziness(new Fuzziness(2))))
                .Sort(sort => sort
                    .Field(f => f, new FieldSort { Order = SortOrder.Desc })));
        if (!result.IsValidResponse)
            return null;
        foreach (var hit in result.Hits) hit!.Source!.Id = hit!.Id!;
        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>?> MatchQueryFullTextAsync(string categoryName)
    {
        var result = await client.SearchAsync<ECommerce>(s => s
            .Index(IndexName)
            .Size(1000)
            .Query(q => q
                .Match(m => m
                    .Field(f => f.Category)
                    .Query(categoryName).Operator(Operator.And))));
        if (!result.IsValidResponse)
            return null;
        foreach (var hit in result.Hits) hit!.Source!.Id = hit!.Id!;
        return result.Documents.ToImmutableList();
    }

    public async Task<ImmutableList<ECommerce>?> MultiMatchQueryFullTextAsync(string name)
    {
        var result = await client.SearchAsync<ECommerce>(s => s
            .Index(IndexName)
            .Size(1000)
            .Query(q => q
                .MultiMatch(mm => mm
                    .Fields(new Field("customer_first_name")
                        .And(new Field("customer_last_name"))
                        .And(new Field("customer_full_name")))
                    .Query(name))));
        if (!result.IsValidResponse)
            return null;
        foreach (var hit in result.Hits) hit!.Source!.Id = hit!.Id!;
        return result.Documents.ToImmutableList();
    }
    
    public async Task<ImmutableList<ECommerce>?> MatchBoolPrefixFullTextAsync(string customerFullName)
    {
        var result = await client.SearchAsync<ECommerce>(s => s
            .Index(IndexName)
            .Size(1000)
            .Query(q => q
                .MatchBoolPrefix(m => m
                    .Field(f => f.CustomerFullName)
                    .Query(customerFullName))));
        if (!result.IsValidResponse)
            return null;
        foreach (var hit in result.Hits) hit!.Source!.Id = hit!.Id!;
        return result.Documents.ToImmutableList();
    }
    
    public async Task<ImmutableList<ECommerce>?> MatchPhrasePrefixFullTextAsync(string customerFullName)
    {
        var result = await client.SearchAsync<ECommerce>(s => s
            .Index(IndexName)
            .Size(1000)
            .Query(q => q
                .MatchPhrasePrefix(m => m
                    .Field(f => f.CustomerFullName)
                    .Query(customerFullName))));
        if (!result.IsValidResponse)
            return null;
        foreach (var hit in result.Hits) hit!.Source!.Id = hit!.Id!;
        return result.Documents.ToImmutableList();
    }
    
    public async Task<ImmutableList<ECommerce>?> CompoundQueryExampleOneAsync(string cityName,double taxFulTotalPrice,string categoryName,string manufacturer)
    {
        var result = await client.SearchAsync<ECommerce>(s => s
            .Index(IndexName)
            .Size(1000)
            .Query(q => q
                .Bool(b => b
                    .Must(m => m
                        .Term(t => t
                            .Field("geoip.city_name")
                            .Value(cityName)))
                    .MustNot(mn => mn
                        .Range(r => r
                            .NumberRange(nr => nr
                                .Field(f => f.TaxFulTotalPrice)
                                .Lte(taxFulTotalPrice))))
                    .Should(queryDescriptor => queryDescriptor
                        .Term(t => t
                            .Field(f => f.Category.Suffix("keyword"))
                            .Value(categoryName)))
                    .Filter(f => f
                        .Term(t => t
                            .Field("manufacturer.keyword")
                            .Value(manufacturer))))
            ));
        if (!result.IsValidResponse)
            return null;
        foreach (var hit in result.Hits) hit!.Source!.Id = hit!.Id!;
        return result.Documents.ToImmutableList();
    }
    
    public async Task<ImmutableList<ECommerce>?> CompoundQueryExampleTwoAsync(string customerFullName)
    {
        // var result = await client.SearchAsync<ECommerce>(s => s
        //     .Index(IndexName)
        // 	.Size(1000)
        //     .Query(q =>q
        //         .MatchPhrasePrefix(m=>m
        //             .Field(f=>f.CustomerFullName).Query(customerFullName))));

        var result = await client.SearchAsync<ECommerce>(s => s
            .Index(IndexName)
            .Size(1000)
            .Query(q => q
                .Bool(b => b
                    .Should(queryDescriptor => queryDescriptor
                        .Match(m => m
                            .Field(f => f.CustomerFullName)
                            .Query(customerFullName)),
                        queryDescriptor => queryDescriptor.Prefix(p => p
                            .Field(f => f.CustomerFullName.Suffix("keyword"))
                            .Value(customerFullName))))));
        
        if (!result.IsValidResponse)
            return null;
        foreach (var hit in result.Hits) hit!.Source!.Id = hit!.Id!;
        return result.Documents.ToImmutableList();
    }
}