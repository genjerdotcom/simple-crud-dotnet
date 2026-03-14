using Microsoft.EntityFrameworkCore;
using Core.Interfaces;

namespace Infrastructure.Repositories;

public class BaseRepository<T> : IRepository<T> where T : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;
    private readonly ILogger<BaseRepository<T>> _logger;

    public BaseRepository(DbContext context, ILogger<BaseRepository<T>> logger)
    {
        _context = context;
        _dbSet = context.Set<T>();
        _logger = logger;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        _logger.LogDebug("Executing GetAllAsync for {Entity}", typeof(T).Name);
        try
        {
            var list = await _dbSet.ToListAsync();
            _logger.LogInformation("Retrieved {Count} {Entity} records", list.Count, typeof(T).Name);
            return list;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync for {Entity}", typeof(T).Name);
            throw;
        }
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        _logger.LogDebug("Executing GetByIdAsync for {Entity} with Id {Id}", typeof(T).Name, id);
        try
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                _logger.LogWarning("{Entity} not found with Id {Id}", typeof(T).Name, id);
            }
            else
            {
                _logger.LogInformation("Retrieved {Entity} with Id {Id}", typeof(T).Name, id);
            }
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetByIdAsync for {Entity} with Id {Id}", typeof(T).Name, id);
            throw;
        }
    }

    public async Task AddAsync(T entity)
    {
        _logger.LogDebug("Executing AddAsync for {Entity}", typeof(T).Name);
        try
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("{Entity} added successfully", typeof(T).Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in AddAsync for {Entity}", typeof(T).Name);
            throw;
        }
    }

    public async Task UpdateAsync(T entity)
    {
        _logger.LogDebug("Executing UpdateAsync for {Entity}", typeof(T).Name);
        try
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("{Entity} updated successfully", typeof(T).Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateAsync for {Entity}", typeof(T).Name);
            throw;
        }
    }

    public async Task DeleteAsync(T entity)
    {
        _logger.LogDebug("Executing DeleteAsync for {Entity}", typeof(T).Name);
        try
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("{Entity} deleted successfully", typeof(T).Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteAsync for {Entity}", typeof(T).Name);
            throw;
        }
    }
}