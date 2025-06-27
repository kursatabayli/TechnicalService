using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.Shared.Services.Implementations
{
    public class SearchService : ISearchService
    {
        public async Task<IEnumerable<T>> SearchAsync<T>(string searchText, List<T> source, Func<T, string> propertySelector, CancellationToken token = default)
            => await Task.Run(() => PerformSearch(searchText, source, propertySelector), token);

        public IEnumerable<T> Search<T>(string searchText, IEnumerable<T> source, Func<T, string> propertySelector)
            => PerformSearch(searchText, source, propertySelector);

        public IEnumerable<T> Search<T>(string searchText, IEnumerable<T> source, Func<T, IEnumerable<string>> propertiesSelector)
        {
            if (source == null) 
                return [];

            return string.IsNullOrWhiteSpace(searchText) ? source : source.Where(item => propertiesSelector(item).Any(prop => prop.Contains(searchText, StringComparison.OrdinalIgnoreCase)));
        }

        private IEnumerable<T> PerformSearch<T>(string searchText, IEnumerable<T> source, Func<T, string> propertySelector)
        {
            if (source == null) 
                return [];

            return string.IsNullOrWhiteSpace(searchText) ? source : source.Where(item => propertySelector(item).Contains(searchText, StringComparison.OrdinalIgnoreCase));
        }
    }
}
