namespace TaskManagement.DAL.UoW;

using TaskManagement.DAL.Data;
using TaskManagement.DAL.Repositories;
using TaskManagement.DAL.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Implements the unit of work pattern to manage repositories and commit changes to the database.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TaskManagementDbContext dbContext;
    private Dictionary<Type, object>? repositories;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="dbContext">The database context for managing entity operations.</param>
    public UnitOfWork(TaskManagementDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    /// Retrieves the repository for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity for which the repository is requested.</typeparam>
    /// <returns>The repository instance for the specified entity type.</returns>
    public IGenericRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class
    {
        if (this.repositories == null)
        {
            this.repositories = new Dictionary<Type, object>();
        }

        var type = typeof(TEntity);
        if (!this.repositories.TryGetValue(type, out object? value))
        {
            value = new GenericRepository<TEntity>(this.dbContext);
            this.repositories[type] = value;
        }

        return (IGenericRepository<TEntity>)value;
    }

    /// <summary>
    /// Commits all changes made in the unit of work to the database asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean value indicating whether the commit was successful.</returns>
    public async Task<bool> CommitAsync()
    {
        var result = await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
        return result > 0;
    }

    /// <summary>
    /// Disposes of the database context and suppresses finalization.
    /// </summary>
    public void Dispose()
    {
        this.dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}