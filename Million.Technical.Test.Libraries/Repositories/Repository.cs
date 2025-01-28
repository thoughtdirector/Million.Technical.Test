using Microsoft.EntityFrameworkCore;

namespace Million.Technical.Test.Libraries.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id) =>
        await _dbSet.FindAsync(id);

    public async Task<IEnumerable<TEntity>?> GetAllAsync() =>
        await _dbSet.ToListAsync();

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error creating {typeof(TEntity).Name} ", ex);
        }
    }

    public async Task UpdateAsync(TEntity entity)
    {
        try
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error updating {typeof(TEntity).Name} ", ex);
        }
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public IQueryable<TEntity> GetQueryable() => _dbSet.AsQueryable();
}