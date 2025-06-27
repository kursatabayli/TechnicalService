using TechnicalService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TechnicalService.Persistence.Context;
using TechnicalService.Application.Contracts.RepositoryContracts;

namespace TechnicalService.Persistence.Repositories.SerialNumberRepositories
{
    public class SerialNumberRepository : ISerialNumberRepository
    {
        private readonly AppDbContext _context;

        public SerialNumberRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SerialNumber>> GetAllSerialNumbersAsync()
            => await _context.SerialNumbers
            .Include(x => x.Product).ThenInclude(x => x.Brand)
            .Include(x => x.Product).ThenInclude(x => x.ProductType)
            .ToListAsync();

        public async Task<SerialNumber> GetSerialNumberByIdAsync(int id)
            => await _context.SerialNumbers
            .Include(x => x.Product).ThenInclude(x => x.Brand)
            .Include(x => x.Product).ThenInclude(x => x.ProductType)
            .FirstOrDefaultAsync(x => x.Id == id);
        
        public async Task<SerialNumber> GetSerialNumberBySerialNumberAsync(string serialNumber)
            => await _context.SerialNumbers
            .Include(x => x.Product)
            .FirstOrDefaultAsync(x => x.Serial_Number == serialNumber);

    }
}
