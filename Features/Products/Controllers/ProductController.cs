using Microsoft.AspNetCore.Mvc;
using Features.Products.DTO;
using Features.Products.UseCases;
using Core.Responses;

namespace Features.Products.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly CreateProductUseCase _createProduct;
    private readonly GetProductsUseCase _getProducts;
    private readonly GetProductByIdUseCase _getProductById;
    private readonly UpdateProductUseCase _updateProduct;
    private readonly DeleteProductUseCase _deleteProduct;

    public ProductController(
        CreateProductUseCase createProduct,
        GetProductsUseCase getProducts,
        GetProductByIdUseCase getProductById,
        UpdateProductUseCase updateProduct,
        DeleteProductUseCase deleteProduct
    )
    {
        _createProduct = createProduct;
        _getProducts = getProducts;
        _getProductById = getProductById;
        _updateProduct = updateProduct;
        _deleteProduct = deleteProduct;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        await _createProduct.Execute(dto);

        return Ok(ApiResponse<string>.SuccessResponse("Product created"));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _getProducts.Execute();

        return Ok(ApiResponse<object>.SuccessResponse(products));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var validator = new Validators.IdValidator();
        var result = validator.Validate(id);
        if (!result.IsValid) return BadRequest(result.Errors.Select(e => e.ErrorMessage));

        var product = await _getProductById.Execute(id);
        if (product == null) return NotFound(ApiResponse<string>.ErrorResponse("Product not found"));
        return Ok(ApiResponse<object>.SuccessResponse(product));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
    {
        await _updateProduct.Execute(id, dto);
        return Ok(ApiResponse<string>.SuccessResponse("Product updated"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _deleteProduct.Execute(id);

        return Ok(ApiResponse<string>.SuccessResponse("Product deleted"));
    }
}