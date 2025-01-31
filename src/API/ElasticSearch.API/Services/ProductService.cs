using System.Net;
using Elastic.Clients.Elasticsearch;
using ElasticSearch.API.DTOs;
using ElasticSearch.API.Repositories;

namespace ElasticSearch.API.Services;

public class ProductService(ProductRepository productRepository, ILogger<ProductService> logger)
{
    public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
    {
        var responseProduct = await productRepository.SaveAsync(request.CreateProduct());
        return responseProduct == null
            ? ResponseDto<ProductDto>.Fail("An error occurred while saving the product", HttpStatusCode.InternalServerError)
            : ResponseDto<ProductDto>.Success(responseProduct.CreateDto(), HttpStatusCode.Created);
    }

    public async Task<ResponseDto<List<ProductDto>>> GetAllAsync()
    {
        var products = await productRepository.GetAllAsync();
        return ResponseDto<List<ProductDto>>.Success(products.Select(x => x.CreateDto()).ToList(), HttpStatusCode.OK);
    }

    public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
    {
        var product = await productRepository.GetByIdAsync(id);
        return product == null
            ? ResponseDto<ProductDto>.Fail("Product not found", HttpStatusCode.NotFound)
            : ResponseDto<ProductDto>.Success(product.CreateDto(), HttpStatusCode.OK);
    }

    public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto updateProduct)
    {
        var isSuccess = await productRepository.UpdateAsync(updateProduct);
        return isSuccess
            ? ResponseDto<bool>.Success(true, HttpStatusCode.NoContent)
            : ResponseDto<bool>.Fail("An error occurred while updating the product", HttpStatusCode.InternalServerError);
    }

    public async Task<ResponseDto<bool>> DeleteAsync(string id)
    {
        var deleteResponse = await productRepository.DeleteAsync(id);
        if (deleteResponse is { IsValidResponse: false, Result: Result.NotFound })
            return ResponseDto<bool>.Fail("Product not found", HttpStatusCode.NotFound);
        if (deleteResponse.IsValidResponse) return ResponseDto<bool>.Success(true, HttpStatusCode.NoContent);

        if (deleteResponse.TryGetOriginalException(out var exception))
            logger.LogError(exception, "An error occurred while deleting the product. Error: {Error}", deleteResponse.ElasticsearchServerError?.Error.Reason);
        return ResponseDto<bool>.Fail("An error occurred while deleting the product", HttpStatusCode.InternalServerError);
    }
}