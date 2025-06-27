using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Text.Json;
using TechnicalService.DTOs.DTOs.TechnicalServiceDTOs;
using TechnicalService.DTOs.Results;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.UserUI.Pages.Contact
{
    public partial class Locations : ComponentBase
    {
        private List<TechnicalServiceDto> _services = [];
        private bool _isLoading = true;
        private bool _isMapInitialized = false;
        private string _searchText = string.Empty;
        private HashSet<int> _copiedItemIds = new();

        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private IDataService<Result<List<TechnicalServiceDto>>> DataService { get; set; }
        [Inject] private ISearchService SearchService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadLocationsAsync();
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

        private async Task LoadLocationsAsync()
        {
            _isLoading = true;
            var result = await DataService.GetAsync(Endpoints.GetAllTechnicalServices, ClientTypes.UserPublicClient);
            if (result.IsSuccess)
            {
                _services = [.. result.Data.OrderBy(x => x.City).ThenBy(x => x.District)];
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
            if (!_isMapInitialized || !_services.Any())
                return;

            await JSRuntime.InvokeVoidAsync("clearMarkers");
            foreach (var service in _services)
            {
                var json = JsonSerializer.Serialize(service);
                await JSRuntime.InvokeVoidAsync("SetMarkers", json);
            }
        }

        private IEnumerable<TechnicalServiceDto> FilteredServices => _isLoading ? [] : SearchService.Search(_searchText, _services, s => [s.ServiceName, s.City, s.District]) ?? [];

        private string GetDirectionsUrl(TechnicalServiceDto service)
        {
            var fullAddress = $"{service.Address}, {service.District} / {service.City}, Posta Kodu: {service.PostalCode}";
            var encodedAddress = Uri.EscapeDataString(fullAddress);
            return $"https://www.google.com/maps/dir/?api=1&destination={encodedAddress}";
        }

        private async Task CopyAddress(TechnicalServiceDto service)
        {
            var addressText = $"{service.Address}, {service.District}, {service.City}, {service.PostalCode}".Trim();
            await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", addressText);
            _copiedItemIds.Add(service.Id);
            StateHasChanged();
            await Task.Delay(2000);
            _copiedItemIds.Remove(service.Id);
            StateHasChanged();
        }
    }
}
