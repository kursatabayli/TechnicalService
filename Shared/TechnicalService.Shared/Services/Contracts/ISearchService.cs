namespace TechnicalService.Shared.Services.Contracts
{
    public interface ISearchService
    {
        Task<IEnumerable<T>> SearchAsync<T>(string searchText, List<T> source, Func<T, string> propertySelector, CancellationToken token = default);

        IEnumerable<T> Search<T>(string searchText, IEnumerable<T> source, Func<T, string> propertySelector );

        IEnumerable<T> Search<T>(string searchText, IEnumerable<T> source, Func<T, IEnumerable<string>> propertiesSelector);
    }
}
