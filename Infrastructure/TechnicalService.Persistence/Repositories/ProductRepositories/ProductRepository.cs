using TechnicalService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TechnicalService.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalService.Application.Contracts.RepositoryContracts;

namespace TechnicalService.Persistence.Repositories.ProductRepositories
{
    public class ProductRepository : GenericRepository<Product, int>, IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync() 
            => await _context.Products
            .Include(x => x.Brand)
            .Include(x => x.ProductType)
            .OrderBy(x => x.BrandId)
            .ThenBy(x => x.ProductTypeId)
            .ThenBy(x => x.ProductName)
            .ToListAsync();

        public async Task<Product> GetProductByIdAsync(int Id)
            => await _context.Products.Include(x => x.Brand).Include(x => x.ProductType).FirstOrDefaultAsync(x => x.Id == Id);

        public async Task<(bool, int)> GetProductByRequestAsync(Product product)
        {
            var existingProduct = await _context.Products.AsNoTracking()
                .Where(x =>
                    x.ProductName.ToLower() == product.ProductName.ToLower() &&
                    x.BrandId == product.BrandId &&
                    x.ProductTypeId == product.ProductTypeId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            bool isExist = existingProduct != default;
            return (isExist, isExist ? existingProduct : product.Id);
        }
    }
}
