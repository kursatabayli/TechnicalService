using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.ProductTypeDTOs;
using TechnicalService.DTOs.Results;
using TechnicalService.Portal.Admin.Messages;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.Portal.Admin.Pages.ProductTypes
{
    public partial class ProductTypeList : ComponentBase
    {
        private List<ProductTypeDto> _productTypes = [];
        private bool _isLoading = true;
        private string _searchText = string.Empty;

        [Inject] private IDataService<ProductTypeDto> DeleteProductType { get; set; }
        [Inject] private IDataService<Result<List<ProductTypeDto>>> GetAllProductTypes { get; set; }
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

            var result = await GetAllProductTypes.GetAsync(Endpoints.GetAllProductTypes, ClientTypes.PersonnelAuthClient);

            if (result.IsSuccess)
            {
                _productTypes = result.Data;
            }
            else
            {
                Snackbar.Add(result.StatusMessage, Severity.Error);
            }

            _isLoading = false;
        }

        private async Task ShowDeleteConfirmationDialogAsync(ProductTypeDto productTypeToDelete)
        {
            if (productTypeToDelete == null) return;

            string title = Localizer[AdminMessages.Delete_Title, AdminMessages.EntityType_ProductType_Singular_Nominative, productTypeToDelete.Type];
            string content = Localizer[AdminMessages.Delete_Content, AdminMessages.EntityType_ProductType_Singular_Accusative];
            string buttonText = Localizer[AdminMessages.Delete_SubmitButtonText];

            bool confirmed = await DeleteDialogService.ShowDeleteDialogAsync(title, content, buttonText);

            if (confirmed)
            {
                await DeleteProductTypeAndNotifyAsync(productTypeToDelete);
            }
        }

        private async Task DeleteProductTypeAndNotifyAsync(ProductTypeDto productTypeToDelete)
        {
            _isLoading = true;
            StateHasChanged();

            var result = await DeleteProductType.DeleteAsync(Endpoints.DeleteProductType + productTypeToDelete.Id, ClientTypes.PersonnelAuthClient);
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

        private IEnumerable<ProductTypeDto> FilteredProductTypes =>
            _isLoading ? [] : SearchService.Search(_searchText, _productTypes, b => b.Type) ?? [];
    }
}