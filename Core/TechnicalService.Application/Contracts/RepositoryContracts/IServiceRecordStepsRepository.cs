using TechnicalService.Domain.Entities;

namespace TechnicalService.Application.Contracts.RepositoryContracts
{
    public interface IServiceRecordStepsRepository : IRepository<ServiceRecordStep, Guid>
    {
        Task<List<ServiceRecordStep>> GetAllServiceRecordStepsWithPersonnelByServiceRecordId(Guid serviceRecordId);
    }
}
