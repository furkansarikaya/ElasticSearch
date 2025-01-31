using ElasticSearch.API.Models;

namespace ElasticSearch.API.DTOs;

public record ProductFeatureDto(int Width, int Height, string Color)
{
    public ProductFeature CreateProductFeature()
    {
        return new ProductFeature
        {
            Width = Width,
            Height = Height,
            Color = (EColor)int.Parse(Color)
        };
    }
}