namespace TaskManagement.DAL.Repositories.IRepository;

using System.Linq.Expressions;

public interface IGenericRepository<T>
    where T : class
{
    Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, string[]? includeOptions = null);

    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string[]? includeOptions = null);

    Task<IEnumerable<T>> GetAllAsync(string[]? includeOptions = null);

    IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, string[]? includeOptions = null);

    IQueryable<T> GetAll(string[]? includeOptions = null);

    bool Add(T entity);

    bool Update(T entity);

    bool Delete(T entity);

    Task<bool> ExistsAsync(Expression<Func<T, bool>> filter);
}