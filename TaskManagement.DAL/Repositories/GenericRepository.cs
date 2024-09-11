namespace TaskManagement.DAL.Repositories;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.DAL.Data;
using TaskManagement.DAL.Repositories.IRepository;

public class GenericRepository<T> : IGenericRepository<T>
    where T : class
{
    private readonly DbSet<T> dbSet;

    public GenericRepository(TaskManagementDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        this.dbSet = dbContext.Set<T>();
    }

    public virtual bool Add(T entity)
    {
        this.dbSet.Add(entity);
        return true;
    }

    public virtual bool Delete(T entity)
    {
        this.dbSet.Remove(entity);
        return true;
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter)
    {
        return await this.dbSet.AnyAsync(filter).ConfigureAwait(false);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string[]? includeOptions = null)
    {
        IQueryable<T> query = this.dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeOptions != null)
        {
            foreach (var entity in includeOptions)
            {
                query = query.Include(entity);
            }
        }

        return await Task.FromResult(query).ConfigureAwait(false);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(string[]? includeOptions = null)
    {
        IQueryable<T> query = this.dbSet;

        if (includeOptions != null)
        {
            foreach (var entity in includeOptions)
            {
                query = query.Include(entity);
            }
        }

        return await Task.FromResult(query).ConfigureAwait(false);
    }

    public virtual IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, string[]? includeOptions = null)
    {
        IQueryable<T> query = this.dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeOptions != null)
        {
            foreach (var entity in includeOptions)
            {
                query = query.Include(entity);
            }
        }

        return query;
    }

    public virtual IQueryable<T> GetAll(string[]? includeOptions = null)
    {
        IQueryable<T> query = this.dbSet;

        if (includeOptions != null)
        {
            foreach (var entity in includeOptions)
            {
                query = query.Include(entity);
            }
        }

        return query;
    }

    public virtual async Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, string[]? includeOptions = null)
    {
        IQueryable<T> query = this.dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeOptions != null)
        {
            foreach (var entity in includeOptions)
            {
                query = query.Include(entity);
            }
        }

        return await query.FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public virtual bool Update(T entity)
    {
        this.dbSet.Update(entity);
        return true;
    }
}