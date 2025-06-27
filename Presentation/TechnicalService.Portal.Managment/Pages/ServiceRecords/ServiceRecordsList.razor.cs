using Microsoft.AspNetCore.Components;
using MudBlazor;
using TechnicalService.DTOs.DTOs.ServiceRecordDTOs;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.Portal.Managment.Pages.ServiceRecords
{
    public partial class ServiceRecordsList : ComponentBase
    {
        private List<ServiceRecordListDto> _serviceRecords = [];
        private bool _isLoading = true;
        private string _searchText = string.Empty;

        [Inject] private IDataService<Result<List<ServiceRecordListDto>>> GetServiceRecordsByServiceId { get; set; }
        [Inject] private ISearchService SearchService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadServiceRecordsAsync();
        }

        private async Task LoadServiceRecordsAsync()
        {
            _isLoading = true;
            var result = await GetServiceRecordsByServiceId.GetAsync(Endpoints.GetServiceRecordsByServiceId, ClientTypes.PersonnelAuthClient);

            if (result.IsSuccess)
            {
                _serviceRecords = result.Data;
            }
            else
            {
                Snackbar.Add(result.StatusMessage, Severity.Error);
            }

            _isLoading = false;
        }

        private void NavigateToPage(string page) => NavigationManager.NavigateTo(page);

        private IEnumerable<ServiceRecordListDto> FilteredServiceRecords 
            => _isLoading ? [] : SearchService.Search(_searchText, _serviceRecords, r => [r.UserFullName, r.SerialNumber, r.Id.ToString()]) ?? [];

        private Color GetStatusColor(ServiceStatusDto status) => status switch
        {
            ServiceStatusDto.Pending => Color.Warning,
            ServiceStatusDto.InProgress => Color.Info,
            ServiceStatusDto.Completed => Color.Success,
            _ => Color.Default
        };
    }
}
