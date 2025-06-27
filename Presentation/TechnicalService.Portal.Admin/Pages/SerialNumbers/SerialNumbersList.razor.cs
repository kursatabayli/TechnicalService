using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.SerialNumberDTOs;
using TechnicalService.DTOs.Results;
using TechnicalService.Portal.Admin.Messages;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.Portal.Admin.Pages.SerialNumbers
{
    public partial class SerialNumbersList : ComponentBase
    {
        private List<SerialNumberDto> _serialNumbers = [];
        private bool _isLoading = true;
        private string _searchText = string.Empty;

        [Inject] private IDataService<SerialNumberDto> DeleteSerialNumber { get; set; }
        [Inject] private IDataService<Result<List<SerialNumberDto>>> GetAllSerialNumbers { get; set; }
        [Inject] private ISearchService SearchService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IDeleteDialogService DeleteDialogService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private IStringLocalizer<AdminMessages> Localizer { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadSerialNumbersAsync();
        }

        private async Task LoadSerialNumbersAsync()
        {
            _isLoading = true;

            var result = await GetAllSerialNumbers.GetAsync(Endpoints.GetAllSerialNumbers, ClientTypes.PersonnelAuthClient);

            if (result.IsSuccess)
            {
                _serialNumbers = result.Data;
            }
            else
            {
                Snackbar.Add(result.StatusMessage, Severity.Error);
            }

            _isLoading = false;
        }

        private async Task ShowDeleteConfirmationDialogAsync(SerialNumberDto serialNumberToDelete)
        {
            if (serialNumberToDelete == null) return;

            string title = Localizer[AdminMessages.Delete_Title, AdminMessages.EntityType_SerialNumber_Singular_Nominative, serialNumberToDelete.Serial_Number];
            string content = Localizer[AdminMessages.Delete_Content, AdminMessages.EntityType_SerialNumber_Singular_Accusative];
            string buttonText = Localizer[AdminMessages.Delete_SubmitButtonText];

            bool confirmed = await DeleteDialogService.ShowDeleteDialogAsync(title, content, buttonText);

            if (confirmed)
            {
                await DeleteSerialNumberAndNotifyAsync(serialNumberToDelete);
            }
        }

        private async Task DeleteSerialNumberAndNotifyAsync(SerialNumberDto serialNumberToDelete)
        {
            _isLoading = true;
            StateHasChanged();

            var result = await DeleteSerialNumber.DeleteAsync(Endpoints.DeleteSerialNumber + serialNumberToDelete.Id, ClientTypes.PersonnelAuthClient);
            var response = await result.Content.ReadFromJsonAsync<Result<int>>();
            if (response.IsSuccess)
            {
                Snackbar.Add(response.StatusMessage, Severity.Success);
                await LoadSerialNumbersAsync();
            }
            else
            {
                Snackbar.Add(response.StatusMessage ?? Localizer[AdminMessages.Delete_ErrorMessage], Severity.Error);
                _isLoading = false;
                StateHasChanged();
            }
        }

        private void NavigateToPage(string page) => NavigationManager.NavigateTo(page);

        private IEnumerable<SerialNumberDto> FilteredSerialNumbers => _isLoading ? [] : SearchService.Search(_searchText, _serialNumbers, s => [s.Serial_Number, s.BrandName, s.ProductName]) ?? [];
    }
}
