using Microsoft.EntityFrameworkCore;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Domain.Entities;
using TechnicalService.Domain.Enums;
using TechnicalService.Persistence.Context;

namespace TechnicalService.Persistence.Repositories.PersonnelRepositories
{
    public class PersonnelRepository : GenericRepository<Personnel, Guid>, IPersonnelRepository
    {
        private readonly AppDbContext _context;
        public PersonnelRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Personnel>> GetAllPersonnelsAsync()
        => await _context.Personnels
            .Include(x => x.TechnicalServices)
            .OrderBy(x =>
            x.PersonnelStatus == PersonnelStatus.Active ? 1 :
            x.PersonnelStatus == PersonnelStatus.OnLeave ? 2 :
            x.PersonnelStatus == PersonnelStatus.Suspended ? 3 :
            x.PersonnelStatus == PersonnelStatus.Terminated ? 4 : 5)
            .ToListAsync();
        public async Task<List<Personnel>> GetPersonnelsByServiceAsync(int serviceId)
        => await _context.Personnels
            .Where(x => x.TechnicalServiceId == serviceId)
            .Include(x => x.TechnicalServices)
            .OrderBy(x =>
            x.PersonnelStatus == PersonnelStatus.Active ? 1 :
            x.PersonnelStatus == PersonnelStatus.OnLeave ? 2 :
            x.PersonnelStatus == PersonnelStatus.Suspended ? 3 :
            x.PersonnelStatus == PersonnelStatus.Terminated ? 4 : 5)
            .ToListAsync();

        public async Task<Personnel> GetPersonnelByIdAsync(Guid Id)
            => await _context.Personnels.Include(x => x.TechnicalServices).FirstOrDefaultAsync(x => x.Id == Id);

    }
}
