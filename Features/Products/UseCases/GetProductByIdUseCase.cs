using Core.Entities;
using Core.Interfaces;

namespace Features.Products.UseCases;

public class GetProductByIdUseCase
{
    private readonly IRepository<Product> _repository;
    private readonly ILogger<GetProductByIdUseCase> _logger;

    public GetProductByIdUseCase(IRepository<Product> repository, ILogger<GetProductByIdUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Product?> Execute(int id)
    {
        _logger.LogDebug("Executing GetProductByIdUseCase for ProductId: {Id}", id);

        try
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
            {
                _logger.LogWarning("Product not found with Id {Id}", id);
                return null;
            }

            _logger.LogInformation("Retrieved product {ProductName} with Id {Id}", product.Name, id);
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving product with Id {Id}", id);
            throw;
        }
    }
}