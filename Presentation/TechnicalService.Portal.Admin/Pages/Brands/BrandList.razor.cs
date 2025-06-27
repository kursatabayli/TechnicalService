using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.BrandDTOs;
using TechnicalService.Portal.Admin.Messages;
using TechnicalService.DTOs.Results;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;


namespace TechnicalService.Portal.Admin.Pages.Brands
{
    public partial class BrandList : ComponentBase
    {
        private List<BrandDto> _brands = [];
        private bool _isLoading = true;
        private string _searchText = string.Empty;

        [Inject] private IDataService<BrandDto> DeleteBrand { get; set; }
        [Inject] private IDataService<Result<List<BrandDto>>> GetAllBrands { get; set; }
        [Inject] private ISearchService SearchService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IDeleteDialogService DeleteDialogService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private IStringLocalizer<AdminMessages> Localizer { get; set; }

        protected override async Task OnInitializedAsync()
        {

            await LoadBrandsAsync();


        }

        private async Task LoadBrandsAsync()
        {
            try
            {
                var result = await GetAllBrands.GetAsync(Endpoints.GetAllBrands, ClientTypes.PersonnelAuthClient);

                if (result.IsSuccess)
                {
                    _brands = [.. result.Data];
                }
                else
                {
                    _brands = [];
                    Snackbar.Add(result.StatusMessage, Severity.Error);
                }
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async Task ShowDeleteConfirmationDialogAsync(BrandDto brandToDelete)
        {
            if (brandToDelete == null) return;

            string title = Localizer[AdminMessages.Delete_Title, AdminMessages.EntityType_Brand_Singular_Nominative, brandToDelete.BrandName];
            string content = Localizer[AdminMessages.Delete_Content, AdminMessages.EntityType_Brand_Singular_Accusative];
            string buttonText = Localizer[AdminMessages.Delete_SubmitButtonText];

            bool confirmed = await DeleteDialogService.ShowDeleteDialogAsync(title, content, buttonText);

            if (confirmed)
            {
                await DeleteBrandAndNotifyAsync(brandToDelete);
            }
        }

        private async Task DeleteBrandAndNotifyAsync(BrandDto brandToDelete)
        {
            _isLoading = true;
            StateHasChanged();

            var result = await DeleteBrand.DeleteAsync(Endpoints.DeleteBrand + brandToDelete.Id, ClientTypes.PersonnelAuthClient);
            var response = await result.Content.ReadFromJsonAsync<Result<int>>();
            if (response.IsSuccess)
            {
                Snackbar.Add(response.StatusMessage, Severity.Success);
                await LoadBrandsAsync();
            }
            else
            {
                Snackbar.Add(response.StatusMessage ?? Localizer[AdminMessages.Delete_ErrorMessage], Severity.Error);
                _isLoading = false;
                StateHasChanged();
            }
        }

        private void NavigateToPage(string page) => NavigationManager.NavigateTo(page);

        private IEnumerable<BrandDto> FilteredBrands =>
            _isLoading ? [] : SearchService.Search(_searchText, _brands, b => b.BrandName) ?? [];
    }
}