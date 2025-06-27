using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MudBlazor;
using System.Net.Http.Json;
using System.Text.Json;
using TechnicalService.DTOs.DTOs.TechnicalServiceDTOs;
using TechnicalService.DTOs.Results;
using TechnicalService.Portal.Admin.Messages;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.Portal.Admin.Pages.TechnicalServices
{
    public partial class TechnicalServicesList : ComponentBase
    {
        private List<TechnicalServiceDto> _technicalServices = [];
        private bool _isLoading = true;
        private string _searchText = string.Empty; 
        private bool _isMapInitialized = false;

        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private IDataService<TechnicalServiceDto> DeleteTechnicalService { get; set; }
        [Inject] private IDataService<Result<List<TechnicalServiceDto>>> GetAllTechnicalServices { get; set; }
        [Inject] private ISearchService SearchService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IDeleteDialogService DeleteDialogService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private IStringLocalizer<AdminMessages> Localizer { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadTechnicalServicesAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("initMap");
                _isMapInitialized = true;
                await UpdateMapMarkersAsync();
            }
        }
        private async Task LoadTechnicalServicesAsync()
        {
            _isLoading = true;
            var result = await GetAllTechnicalServices.GetAsync(Endpoints.GetAllTechnicalServices, ClientTypes.PersonnelAuthClient);

            if (result.IsSuccess)
            {
                _technicalServices = result.Data;
            }
            else
            {
                Snackbar.Add(result.StatusMessage, Severity.Error);
            }

            _isLoading = false;

            await UpdateMapMarkersAsync();
            StateHasChanged();
        }
        private async Task UpdateMapMarkersAsync()
        {
            if (!_isMapInitialized)
                return;

            await JSRuntime.InvokeVoidAsync("clearMarkers");

            foreach (var service in _technicalServices)
            {
                var json = JsonSerializer.Serialize(service);
                await JSRuntime.InvokeVoidAsync("SetMarkers", json);
            }
        }
        private async Task ShowDeleteConfirmationDialogAsync(TechnicalServiceDto technicalServiceToDelete)
        {
            if (technicalServiceToDelete == null) return;

            string title = Localizer[AdminMessages.Delete_Title, AdminMessages.EntityType_TechnicalService_Singular_Nominative, technicalServiceToDelete.ServiceName];
            string content = Localizer[AdminMessages.Delete_Content, AdminMessages.EntityType_TechnicalService_Singular_Accusative];
            string buttonText = Localizer[AdminMessages.Delete_SubmitButtonText];

            bool confirmed = await DeleteDialogService.ShowDeleteDialogAsync(title, content, buttonText);

            if (confirmed)
            {
                await DeleteTechnicalServiceAndNotifyAsync(technicalServiceToDelete);
            }
        }

        private async Task DeleteTechnicalServiceAndNotifyAsync(TechnicalServiceDto technicalServiceToDelete)
        {
            _isLoading = true;
            StateHasChanged();

            var result = await DeleteTechnicalService.DeleteAsync(Endpoints.DeleteTechnicalService + technicalServiceToDelete.Id, ClientTypes.PersonnelAuthClient);
            var response = await result.Content.ReadFromJsonAsync<Result<int>>();
            if (response.IsSuccess)
            {
                Snackbar.Add(response.StatusMessage, Severity.Success);
                await LoadTechnicalServicesAsync();
            }
            else
            {
                Snackbar.Add(response.StatusMessage ?? Localizer[AdminMessages.Delete_ErrorMessage], Severity.Error);
                _isLoading = false;
                StateHasChanged();
            }
        }

        private void NavigateToPage(string page) => NavigationManager.NavigateTo(page);

        private IEnumerable<TechnicalServiceDto> FilteredTechnicalServices => _isLoading ? [] : SearchService.Search(_searchText, _technicalServices, t => [t.ServiceName, t.City, t.District]) ?? [];
    }
}
