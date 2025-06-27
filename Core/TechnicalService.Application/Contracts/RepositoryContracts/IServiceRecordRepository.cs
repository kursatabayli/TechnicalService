using TechnicalService.Domain.Entities;

namespace TechnicalService.Application.Contracts.RepositoryContracts
{
    public interface IServiceRecordRepository : IRepository<ServiceRecord, Guid>
    {
        Task<ServiceRecord?> GetServiceRecordByIdAsync(Guid id);
        Task<List<ServiceRecord>> GetAllServiceRecordsAsync();
        Task<List<ServiceRecord>> GetAllServiceRecordsByUserIdAsync(Guid userId);
        Task<List<ServiceRecord>> GetAllServiceRecordsByPersonnelIdAsync(Guid personnelId);
        Task<List<ServiceRecord>> SearchServiceRecordQuery(string searchTerm);
        Task<List<ServiceRecord>> GetServiceRecordsByServiceIdAsync(int technicalServiceId);
    }
}
