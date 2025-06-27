using TechnicalService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalService.Application.Contracts.RepositoryContracts
{
    public interface IProductRepository : IRepository<Product, int>
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int Id);
    }
}
