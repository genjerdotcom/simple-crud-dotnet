using Core.Entities;
using Core.Interfaces;

namespace Features.Products.UseCases;

public class GetProductsUseCase
{
    private readonly IRepository<Product> _repository;
    private readonly ILogger<GetProductsUseCase> _logger;

    public GetProductsUseCase(IRepository<Product> repository, ILogger<GetProductsUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<Product>> Execute()
    {
        _logger.LogDebug("Executing GetProductsUseCase");

        try
        {
            var products = await _repository.GetAllAsync();
            _logger.LogInformation("Retrieved {Count} products", products.Count());
            return products;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving all products");
            throw;
        }
    }
}