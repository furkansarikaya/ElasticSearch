using ElasticSearch.API.DTOs;

namespace ElasticSearch.API.Models;

public class Product
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }

    public ProductFeature? Feature { get; set; }

    public ProductDto CreateDto()
    {
        return Feature == null ? new ProductDto(Id,Name,Price,Stock) : new ProductDto(Id,Name,Price,Stock,Feature.CreateDto());
    }
}