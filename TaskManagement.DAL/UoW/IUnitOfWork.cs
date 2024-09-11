namespace TaskManagement.DAL.UoW;

using TaskManagement.DAL.Repositories.IRepository;
using System.Threading.Tasks;

/// <summary>
/// Defines the contract for a unit of work, which manages repository instances and commits changes to the database.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Retrieves the repository for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity for which the repository is requested.</typeparam>
    /// <returns>The repository instance for the specified entity type.</returns>
    IGenericRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class;

    /// <summary>
    /// Commits all changes made in the unit of work to the database asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean value indicating whether the commit was successful.</returns>
    Task<bool> CommitAsync();
}