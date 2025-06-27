using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.ProductDTOs;
using TechnicalService.DTOs.Results;
using TechnicalService.Portal.Admin.Messages;
using TechnicalService.RazorHelpers.Shared;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.Portal.Admin.Pages.Products
{
    public partial class ProductList : ComponentBase
    {
        private List<ProductDto> ProductDto = [];
        private bool _isLoading = true;
        private string SearchText = string.Empty;

        [Inject] private IDataService<ProductDto> DeleteProduct { get; set; }
        [Inject] private IDataService<Result<List<ProductDto>>> GetAllProducts { get; set; }
        [Inject] private ISearchService SearchService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IDeleteDialogService DeleteDialogService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private IStringLocalizer<AdminMessages> Localizer { get; set; }

        protected override async Task OnInitializedAsync()
        {

            await LoadProductsAsync();

        }

        private async Task LoadProductsAsync()
        {
            try
            {
                var result = await GetAllProducts.GetAsync(Endpoints.GetAllProducts, ClientTypes.PersonnelAuthClient);

                if (result.IsSuccess)
                    ProductDto = [.. result.Data];
                else
                    Snackbar.Add(result.StatusMessage, Severity.Error);
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async Task ShowDeleteConfirmationDialogAsync(ProductDto productToDelete)
        {
            if (productToDelete == null) return;

            string title = Localizer[AdminMessages.Delete_Title, AdminMessages.EntityType_Product_Singular_Nominative, productToDelete.ProductName];
            string content = Localizer[AdminMessages.Delete_Content, AdminMessages.EntityType_Product_Singular_Accusative];
            string buttonText = Localizer[AdminMessages.Delete_SubmitButtonText];

            bool confirmed = await DeleteDialogService.ShowDeleteDialogAsync(title, content, buttonText);

            if (confirmed)
            {
                await DeleteProductAndNotifyAsync(productToDelete);
            }
        }

        private async Task DeleteProductAndNotifyAsync(ProductDto productToDelete)
        {
            _isLoading = true;
            StateHasChanged();

            var result = await DeleteProduct.DeleteAsync(Endpoints.DeleteProduct + productToDelete.Id, ClientTypes.PersonnelAuthClient);
            var response = await result.Content.ReadFromJsonAsync<Result<int>>();
            if (response.IsSuccess)
            {
                Snackbar.Add(response.StatusMessage, Severity.Success);
                await LoadProductsAsync();
            }
            else
            {
                Snackbar.Add(response.StatusMessage ?? Localizer[AdminMessages.Delete_ErrorMessage], Severity.Error);
                _isLoading = false;
                StateHasChanged();
            }
        }

        private void NavigateToPage(string page) => NavigationManager.NavigateTo(page);

        private IEnumerable<ProductDto> FilteredProducts => SearchService.Search(SearchText, ProductDto, b => [b.ProductName, b.BrandName, b.Type]) ?? [];
    }
}
