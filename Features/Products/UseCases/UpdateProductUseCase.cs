using Core.Entities;
using Core.Interfaces;
using Features.Products.DTO;

namespace Features.Products.UseCases;

public class UpdateProductUseCase
{
    private readonly IRepository<Product> _repository;
    private readonly ILogger<UpdateProductUseCase> _logger;

    public UpdateProductUseCase(IRepository<Product> repository, ILogger<UpdateProductUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Execute(int id, UpdateProductDto dto)
    {
        _logger.LogDebug("Executing UpdateProductUseCase for ProductId {Id} with {@Dto}", id, dto);

        try
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
            {
                _logger.LogWarning("Product not found with Id {Id}", id);
                throw new Exception("Product not found");
            }

            product.Name = dto.Name;
            product.Price = dto.Price ?? product.Price;

            await _repository.UpdateAsync(product);

            _logger.LogInformation("Updated product {ProductName} with Id {Id}", product.Name, id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product with Id {Id}", id);
            throw;
        }
    }
}