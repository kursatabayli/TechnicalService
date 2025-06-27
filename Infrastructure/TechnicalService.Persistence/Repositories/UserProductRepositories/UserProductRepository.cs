using TechnicalService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TechnicalService.Persistence.Context;
using TechnicalService.Application.Contracts.RepositoryContracts;

namespace TechnicalService.Persistence.Repositories.UserProductRepositories
{
    public class UserProductRepository : IUserProductRepository
    {
        private readonly AppDbContext _context;

        public UserProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserProduct>> GetAllUserProductsAsync()
            => await _context.UserProducts
            .Include(x => x.SerialNumber)
                .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.Brand)
            .Include(x => x.SerialNumber)
                .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.ProductType)
            .ToListAsync();

        public async Task<UserProduct> GetUserProductByIdAsync(int id)
            => await _context.UserProducts
            .Include(x => x.SerialNumber)
                .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.Brand)
            .Include(x => x.SerialNumber)
                .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.ProductType)
            .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<UserProduct>> GetUsersProductsByUserIdAsync(Guid userId)
            => await _context.UserProducts
            .Where(x => x.UserId == userId)
            .Include(x => x.SerialNumber)
                .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.Brand)
            .Include(x => x.SerialNumber)
                .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.ProductType)
            .ToListAsync();
    }
}
