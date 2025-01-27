namespace Million.Technical.Test.Libraries.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(Guid id);

    Task<IEnumerable<TEntity>?> GetAllAsync();

    Task<TEntity> CreateAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(TEntity entity);

    IQueryable<TEntity> GetQueryable();
}