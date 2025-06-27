namespace TechnicalService.Shared.Services.Contracts
{
    public interface IDataService<T> where T : class
    {
        Task<T> GetAsync(string endpoint, string clientType);
        Task<IEnumerable<T>> GetListAsync(string endpoint, string clientType);
        Task<HttpResponseMessage> CreateAsync(string endpoint, T data, string clientType);
        Task<HttpResponseMessage> UpdateAsync(string endpoint, T data, string clientType);
        Task<HttpResponseMessage> DeleteAsync(string endpoint, string clientType);

    }
}
