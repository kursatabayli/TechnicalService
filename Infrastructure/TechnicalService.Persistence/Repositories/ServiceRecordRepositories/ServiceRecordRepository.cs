using Microsoft.EntityFrameworkCore;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Domain.Entities;
using TechnicalService.Persistence.Context;

namespace TechnicalService.Persistence.Repositories.ServiceRecordRepositories
{
    public class ServiceRecordRepository : GenericRepository<ServiceRecord, Guid>, IServiceRecordRepository
    {
        private readonly AppDbContext _context;

        public ServiceRecordRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ServiceRecord?> GetServiceRecordByIdAsync(Guid id)
            => await _context.ServiceRecords
                .Include(x => x.Personnel).FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<ServiceRecord>> GetAllServiceRecordsAsync()
            => await _context.ServiceRecords
                .Include(x => x.UserProduct)
                    .ThenInclude(up => up.SerialNumber)
                        .ThenInclude(sn => sn.Product)
                            .ThenInclude(p => p.Brand)
            .Include(x => x.User)
            .OrderByDescending(x => x.CreatedDate).ToListAsync();

        public async Task<List<ServiceRecord>> GetAllServiceRecordsByUserIdAsync(Guid userId)
            => await _context.ServiceRecords
                .Include(x => x.UserProduct)
                    .ThenInclude(up => up.SerialNumber)
                        .ThenInclude(sn => sn.Product)
                            .ThenInclude(p => p.Brand)
                .Include(x => x.UserProduct)
                    .ThenInclude(up => up.SerialNumber)
                        .ThenInclude(sn => sn.Product)
                            .ThenInclude(p => p.ProductType)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        public async Task<List<ServiceRecord>> GetAllServiceRecordsByPersonnelIdAsync(Guid personnelId)
            => await _context.ServiceRecords
                .Include(x => x.UserProduct)
                    .ThenInclude(up => up.SerialNumber)
                        .ThenInclude(sn => sn.Product)
                            .ThenInclude(p => p.Brand)
                .Include(x => x.UserProduct)
                    .ThenInclude(up => up.SerialNumber)
                        .ThenInclude(sn => sn.Product)
                            .ThenInclude(p => p.ProductType)
                .Include(x => x.User)
                .Where(x => x.PersonnelId == personnelId)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();


        public async Task<List<ServiceRecord>> SearchServiceRecordQuery(string searchTerm)
        {
            var trimmedSearchTerm = searchTerm.Trim();

            var query = _context.ServiceRecords
                .Include(sr => sr.User)
                .Include(sr => sr.UserProduct)
                    .ThenInclude(up => up.SerialNumber)
                        .ThenInclude(sn => sn.Product)
                            .ThenInclude(p => p.Brand)
                .AsQueryable();

            query = query.Where(sr =>
                sr.Id.ToString().StartsWith(trimmedSearchTerm) ||
                sr.UserProduct.SerialNumber.Serial_Number == trimmedSearchTerm ||
                sr.User.PhoneNumber == trimmedSearchTerm);

            return await query.OrderByDescending(sr => sr.CreatedDate).ToListAsync();
        }

        public async Task<List<ServiceRecord>> GetServiceRecordsByServiceIdAsync(int technicalServiceId)
        {
            var serviceRecords = await _context.ServiceRecords
                .Include(sr => sr.Personnel)
                .Where(sr => sr.Personnel != null && sr.Personnel.TechnicalServiceId == technicalServiceId)
                .ToListAsync();

            return serviceRecords;
        }
    }
}
