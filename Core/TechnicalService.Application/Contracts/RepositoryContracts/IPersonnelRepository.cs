using TechnicalService.Domain.Entities;

namespace TechnicalService.Application.Contracts.RepositoryContracts
{
    public interface IPersonnelRepository : IRepository<Personnel, Guid>
    {
        Task<List<Personnel>> GetAllPersonnelsAsync();
        Task<List<Personnel>> GetPersonnelsByServiceAsync(int serviceId);
        Task<Personnel> GetPersonnelByIdAsync(Guid Id);
    }
}
