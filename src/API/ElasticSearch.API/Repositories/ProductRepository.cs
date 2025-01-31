using System.Collections.Immutable;
using ElasticSearch.API.DTOs;
using ElasticSearch.API.Models;
using Nest;

namespace ElasticSearch.API.Repositories;

public class ProductRepository(ElasticClient client)
{
    private const string IndexName = "products";
    public async Task<Product?> SaveAsync(Product newProduct)
    {
        newProduct.Created = DateTime.Now;
        var response = await client.IndexAsync(newProduct,x => x.Index(IndexName).Id(Guid.NewGuid().ToString()));
        
        //fast fail
        if(!response.IsValid) return null;
        
        newProduct.Id = response.Id;
        return newProduct;
    }
    
    public async Task<ImmutableList<Product>> GetAllAsync()
    {
        var result = await client.SearchAsync<Product>(x => 
            x.Index(IndexName)
                .Query(q => 
                    q.MatchAll()));
        foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
        return result.Documents.ToImmutableList();
    }

    public async Task<Product?> GetByIdAsync(string id)
    {
        var response = await client.GetAsync<Product>(id, x => x.Index(IndexName));
        if (!response.IsValid) return null;
        response.Source.Id = response.Id;
        return response.Source;
    }
    
    public async Task<bool> UpdateAsync(ProductUpdateDto updateProduct)
    {
        var response = await client.UpdateAsync<Product,ProductUpdateDto>(updateProduct.Id, x => x
            .Index(IndexName)
            .Doc(updateProduct));
        return response.IsValid;
    }
    
    /// <summary>
    /// Hata yonetimi icin bu method ele alinmistir.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DeleteResponse> DeleteAsync(string id)
    {
        var response = await client.DeleteAsync<Product>(id, x => x.Index(IndexName));
        return response;
    }
}