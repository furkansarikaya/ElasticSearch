using ElasticSearch.API.Controllers.Base;
using ElasticSearch.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearch.API.Controllers;

public class ECommerceController(ECommerceRepository eCommerceRepository) : BaseController
{
    [HttpGet("term-level-query/{customerFirstName}")]
    public async Task<IActionResult> TermLevelQueryAsync(string customerFirstName)
    {
        return Ok(await eCommerceRepository.TermLevelQueryAsync(customerFirstName));
    }

    [HttpPost("terms-query")]
    public async Task<IActionResult> TermsQueryAsync([FromBody] List<string> customerFirstNameList)
    {
        return Ok(await eCommerceRepository.TermsQueryAsync(customerFirstNameList));
    }

    [HttpGet("prefix-query/{customerFullName}")]
    public async Task<IActionResult> PrefixQueryAsync(string customerFullName)
    {
        return Ok(await eCommerceRepository.PrefixQueryAsync(customerFullName));
    }
    
    [HttpGet("range-query/{fromPrice}/{toPrice}")]
    public async Task<IActionResult> RangeQueryAsync(double fromPrice, double toPrice)
    {
        return Ok(await eCommerceRepository.RangeQueryAsync(fromPrice, toPrice));
    }
    
    [HttpGet("match-all-query/{size}")]
    public async Task<IActionResult> MatchAllQueryAsync(int size)
    {
        return Ok(await eCommerceRepository.MatchAllQueryAsync(size));
    }
    
    [HttpGet("pagination-query/{page}/{pageSize}")]
    public async Task<IActionResult> PaginationQueryAsync(int page = 1, int pageSize = 10)
    {
        return Ok(await eCommerceRepository.PaginationQueryAsync(page, pageSize));
    }
    
    [HttpGet("wildcard-query/{customerFullName}")]
    public async Task<IActionResult> WildcardQueryAsync(string customerFullName)
    {
        return Ok(await eCommerceRepository.WildcardQueryAsync(customerFullName));
    }
    
    [HttpGet("fuzzy-query/{customerName}")]
    public async Task<IActionResult> FuzzyQueryAsync(string customerName)
    {
        return Ok(await eCommerceRepository.FuzzyQueryAsync(customerName));
    }
    
    [HttpGet("match-query-full-text/{categoryName}")]
    public async Task<IActionResult> MatchQueryFullTextAsync(string categoryName)
    {
        return Ok(await eCommerceRepository.MatchQueryFullTextAsync(categoryName));
    }
    
    [HttpGet("multi-match-query-full-text/{name}")]
    public async Task<IActionResult> MultiMatchQueryFullTextAsync(string name)
    {
        return Ok(await eCommerceRepository.MultiMatchQueryFullTextAsync(name));
    }
    
    [HttpGet("match-bool-prefix-full-text/{customerFullName}")]
    public async Task<IActionResult> MatchBoolPrefixFullTextAsync(string customerFullName)
    {
        return Ok(await eCommerceRepository.MatchBoolPrefixFullTextAsync(customerFullName));
    }
    
    [HttpGet("match-phrase-prefix-full-text/{customerFullName}")]
    public async Task<IActionResult> MatchPhrasePrefixFullTextAsync(string customerFullName)
    {
        return Ok(await eCommerceRepository.MatchPhrasePrefixFullTextAsync(customerFullName));
    }
    
    [HttpGet("compound-query-example-one/{cityName}/{taxFulTotalPrice}/{categoryName}/{manufacturer}")]
    public async Task<IActionResult> CompoundQueryExampleOneAsync(string cityName,double taxFulTotalPrice,string categoryName,string manufacturer)
    {
        return Ok(await eCommerceRepository.CompoundQueryExampleOneAsync(cityName,taxFulTotalPrice,categoryName,manufacturer));
    }
    
    [HttpGet("compound-query-example-two/{customerFullName}")]
    public async Task<IActionResult> CompoundQueryExampleTwoAsync(string customerFullName)
    {
        return Ok(await eCommerceRepository.CompoundQueryExampleTwoAsync(customerFullName));
    }
    
    
    
}