using Microsoft.EntityFrameworkCore.Storage;
using TechnicalService.Persistence.Context;
using System.Collections.Concurrent;
using TechnicalService.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;

namespace TechnicalService.Persistence.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private ConcurrentDictionary<Type, object> _repositories;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _repositories = new ConcurrentDictionary<Type, object>();
        }

        public IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : class
        {
            var type = typeof(TEntity);
            return (IRepository<TEntity, TKey>)_repositories.GetOrAdd(type, t => new GenericRepository<TEntity, TKey>(_context));
        }
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task SaveChangesWithTransactionAsync()
        {
            var executionStrategy = _context.Database.CreateExecutionStrategy();

            await executionStrategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    if (transaction != null)
                        await transaction.DisposeAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    if (transaction != null)
                        await transaction.DisposeAsync();
                    throw new Exception("Error saving changes", ex);
                }
            });
        }

        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}

