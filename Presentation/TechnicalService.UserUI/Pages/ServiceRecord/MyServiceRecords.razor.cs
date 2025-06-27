using Microsoft.AspNetCore.Components;
using MudBlazor;
using TechnicalService.DTOs.DTOs.ServiceRecordDTOs;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.UserUI.Pages.ServiceRecord
{
    public partial class MyServiceRecords : ComponentBase
    {
        private List<UserServiceRecordsDto> dto;
        private bool LoadingData { get; set; } = true;
        private string SearchText { get; set; } = string.Empty;
        [Inject] private IDataService<Result<List<UserServiceRecordsDto>>> GetUserServiceRecordDataService { get; set; }
        [Inject] private ISearchService SearchService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var serviceRecords = await GetUserServiceRecordDataService.GetAsync(Endpoints.GetUserServiceRecordsByUser, ClientTypes.UserAuthClient);
                dto = serviceRecords.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
            }
            LoadingData = false;

        }

        private IEnumerable<UserServiceRecordsDto> FilteredRequests =>
            SearchService.Search(SearchText, dto, b => [b.Serial_Number, b.ProductName]) ?? [];
        private Color GetStatusColor(ServiceStatusDto status)
            => status switch
            {
                ServiceStatusDto.Pending => Color.Inherit,
                ServiceStatusDto.InProgress => Color.Warning,
                ServiceStatusDto.Completed => Color.Success,
                _ => throw new NotImplementedException(),
            };
        private void NavigatePage(string page) => NavigationManager.NavigateTo(page);
    }
}
