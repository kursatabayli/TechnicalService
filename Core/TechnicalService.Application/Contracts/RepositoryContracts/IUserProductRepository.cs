using TechnicalService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalService.Application.Contracts.RepositoryContracts
{
    public interface IUserProductRepository
    {
        Task<List<UserProduct>> GetAllUserProductsAsync();
        Task<UserProduct> GetUserProductByIdAsync(int id);
        Task<List<UserProduct>> GetUsersProductsByUserIdAsync(Guid userid);
    }
}
