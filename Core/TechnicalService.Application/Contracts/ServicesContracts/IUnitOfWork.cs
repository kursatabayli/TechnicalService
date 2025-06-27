using Microsoft.EntityFrameworkCore.Storage;
using TechnicalService.Application.Contracts.RepositoryContracts;

namespace TechnicalService.Application.Contracts.ServicesContracts
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : class;
        Task SaveChangesAsync();
        Task SaveChangesWithTransactionAsync();
    }
}
