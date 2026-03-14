namespace Features.Products.UseCases;

using Core.Entities;
using Core.Interfaces;
using Features.Products.DTO;
using Microsoft.Extensions.Logging;

public class CreateProductUseCase
{
    private readonly IRepository<Product> _repository;
    private readonly ILogger<CreateProductUseCase> _logger;

    public CreateProductUseCase(
        IRepository<Product> repository,
        ILogger<CreateProductUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Execute(CreateProductDto dto)
    {
        _logger.LogDebug("Start creating product {@Dto}", dto);

        try
        {
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price
            };

            _logger.LogTrace("Mapped DTO to Product entity {@Product}", product);

            await _repository.AddAsync(product);

            _logger.LogInformation(
                "Product created successfully {ProductName} with price {Price}",
                product.Name,
                product.Price
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error occurred while creating product {ProductName}",
                dto.Name
            );

            throw;
        }
    }
}