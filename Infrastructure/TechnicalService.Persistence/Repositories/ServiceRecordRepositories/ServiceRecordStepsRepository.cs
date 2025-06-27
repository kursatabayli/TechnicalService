using Microsoft.EntityFrameworkCore;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Domain.Entities;
using TechnicalService.Persistence.Context;

namespace TechnicalService.Persistence.Repositories.ServiceRecordRepositories
{
    public class ServiceRecordStepsRepository : GenericRepository<ServiceRecordStep, Guid>, IServiceRecordStepsRepository
    {
        private readonly AppDbContext _context;

        public ServiceRecordStepsRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ServiceRecordStep>> GetAllServiceRecordStepsWithPersonnelByServiceRecordId(Guid serviceRecordId)
            => await _context.ServiceRecordSteps.Include(x => x.Personnel).Where(x => x.ServiceRecordId == serviceRecordId).OrderBy(x => x.Order).ToListAsync();
    }
}
