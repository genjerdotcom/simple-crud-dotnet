using Core.Entities;
using Infrastructure.Database;
using Infrastructure.Repositories;

namespace Features.Products.Repository;

public class ProductRepository : BaseRepository<Product>
{
    public ProductRepository(AppDbContext context, ILogger<BaseRepository<Product>> logger)
        : base(context, logger)
    {
    }
}