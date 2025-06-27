using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.LegalDocumentDTOs;
using TechnicalService.DTOs.DTOs.ProductTypeDTOs;
using TechnicalService.DTOs.Results;
using TechnicalService.Portal.Admin.Messages;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Extensions;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.Portal.Admin.Pages.LegalDocuments
{
    public partial class LegalDocumentsList : ComponentBase
    {
        private List<LegalDocumentDto> legalDocumentDtos = [];
        private bool _isLoading = true;
        private string _searchText = string.Empty;

        [Inject] private IDataService<LegalDocumentDto> DeleteLegalDocument { get; set; }
        [Inject] private IDataService<Result<List<LegalDocumentDto>>> GetAllLegalDocuments { get; set; }
        [Inject] private ISearchService SearchService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IDeleteDialogService DeleteDialogService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private IStringLocalizer<AdminMessages> Localizer { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadProductTypesAsync();
        }

        private async Task LoadProductTypesAsync()
        {
            _isLoading = true;

            var result = await GetAllLegalDocuments.GetAsync(Endpoints.GetAllLegalDocuments, ClientTypes.PersonnelAuthClient);

            if (result.IsSuccess)
            {
                legalDocumentDtos = result.Data;
            }
            else
            {
                Snackbar.Add(result.StatusMessage, Severity.Error);
            }

            _isLoading = false;
        }

        private async Task ShowDeleteConfirmationDialogAsync(LegalDocumentDto legalDocumentDto)
        {
            if (legalDocumentDto == null) return;

            string title = Localizer[AdminMessages.Delete_Title, AdminMessages.EntityType_LegalDocument_Singular_Nominative, legalDocumentDto.DocumentType.GetDescription()];
            string content = Localizer[AdminMessages.Delete_Content, AdminMessages.EntityType_LegalDocument_Singular_Accusative];
            string buttonText = Localizer[AdminMessages.Delete_SubmitButtonText];

            bool confirmed = await DeleteDialogService.ShowDeleteDialogAsync(title, content, buttonText);

            if (confirmed)
            {
                await DeleteProductTypeAndNotifyAsync(legalDocumentDto);
            }
        }

        private async Task DeleteProductTypeAndNotifyAsync(LegalDocumentDto legalDocumentDto)
        {
            _isLoading = true;
            StateHasChanged();

            var result = await DeleteLegalDocument.DeleteAsync(Endpoints.DeleteProductType + legalDocumentDto.Id, ClientTypes.PersonnelAuthClient);
            var response = await result.Content.ReadFromJsonAsync<Result<int>>();
            if (response.IsSuccess)
            {
                Snackbar.Add(response.StatusMessage, Severity.Success);
                await LoadProductTypesAsync();
            }
            else
            {
                Snackbar.Add(response.StatusMessage ?? Localizer[AdminMessages.Delete_ErrorMessage], Severity.Error);
                _isLoading = false;
                StateHasChanged();
            }
        }

        private void NavigateToPage(string page) => NavigationManager.NavigateTo(page);

        private IEnumerable<LegalDocumentDto> FilteredProductTypes =>
            _isLoading ? [] : SearchService.Search(_searchText, legalDocumentDtos, b => b.DocumentType.GetDescription()) ?? [];
    }
}
