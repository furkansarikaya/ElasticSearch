using ElasticSearch.API.Models;

namespace ElasticSearch.API.DTOs;

public record ProductUpdateDto(string Id, string Name, decimal Price, int Stock, ProductFeatureDto? Feature)
{
    public Product CreateProduct()
    {
        return new Product
        {
            Id = Id,
            Name = Name,
            Price = Price,
            Stock = Stock,
            Feature = Feature?.CreateProductFeature()
        };
    }
}