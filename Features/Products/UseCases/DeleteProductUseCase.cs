using Core.Entities;
using Core.Interfaces;

namespace Features.Products.UseCases;

public class DeleteProductUseCase
{
    private readonly IRepository<Product> _repository;
    private readonly ILogger<DeleteProductUseCase> _logger;

    public DeleteProductUseCase(IRepository<Product> repository, ILogger<DeleteProductUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Execute(int id)
    {
        _logger.LogDebug("Executing DeleteProductUseCase for ProductId {Id}", id);

        try
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
            {
                _logger.LogWarning("Product not found with Id {Id}", id);
                throw new Exception("Product not found");
            }

            await _repository.DeleteAsync(product);

            _logger.LogInformation("Deleted product {ProductName} with Id {Id}", product.Name, id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product with Id {Id}", id);
            throw;
        }
    }
}