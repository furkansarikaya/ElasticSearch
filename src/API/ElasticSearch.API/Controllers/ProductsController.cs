using ElasticSearch.API.Controllers.Base;
using ElasticSearch.API.DTOs;
using ElasticSearch.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearch.API.Controllers;

public class ProductsController(ProductService productService) : BaseController
{
    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="request">The product creation request data.</param>
    /// <returns>A response indicating the result of the product creation.</returns>
    /// <response code="201">Returns the created product.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ResponseDto<ProductDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseDto<ProductDto>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SaveAsync(ProductCreateDto request)
    {
        return CreateActionResult(await productService.SaveAsync(request));
    }
   
    /// <summary>
    /// Retrieves a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to retrieve.</param>
    /// <returns>A response containing the product details.</returns>
    /// <response code="200">Returns the product details.</response>
    /// <response code="404">If the product is not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseDto<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<ProductDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        return CreateActionResult(await productService.GetByIdAsync(id));
    }
    
    /// <summary>
    /// Retrieves all products.
    /// </summary>
    /// <returns>A response containing a list of all products.</returns>
    /// <response code="200">Returns the list of products.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ResponseDto<List<ProductDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDto<List<ProductDto>>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAsync()
    {
        return CreateActionResult(await productService.GetAllAsync());
    }
    
    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <param name="updateProduct">The product update request data.</param>
    /// <returns>A response indicating the result of the product update.</returns>
    /// <response code="204">If the product was successfully updated.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpPut]
    [ProducesResponseType(typeof(ResponseDto<bool>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseDto<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync(ProductUpdateDto updateProduct)
    {
        return CreateActionResult(await productService.UpdateAsync(updateProduct));
    }
    
    /// <summary>
    /// Deletes a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to delete.</param>
    /// <returns>A response indicating the result of the product deletion.</returns>
    /// <response code="204">If the product was successfully deleted.</response>
    /// <response code="404">If the product is not found.</response>
    /// <response code="500">If there is an internal server error.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ResponseDto<bool>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseDto<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseDto<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        return CreateActionResult(await productService.DeleteAsync(id));
    }
}