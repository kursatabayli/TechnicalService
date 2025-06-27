using Microsoft.AspNetCore.Components;
using MudBlazor;
using TechnicalService.DTOs.DTOs.UserProductDTOs;
using TechnicalService.DTOs.Results;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.UserUI.Pages.UserProducts
{
    public partial class MyProducts : ComponentBase
    {
        private List<UserProductDto> dto = [];
        private string SearchText { get; set; } = string.Empty;
        private bool ShowDetails { get; set; } = false;
        private bool LoadingData { get; set; } = true;
        [Inject] private IDataService<Result<List<UserProductDto>>> dataService { get; set; }
        [Inject] private ISearchService SearchService { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await dataService.GetAsync(Endpoints.GetUserProducts, ClientTypes.UserAuthClient);
                if (result.IsSuccess)
                {
                    dto = result.Data;
                }
                else
                {
                    Snackbar.Add(result.StatusMessage, Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
            }
            LoadingData = false;
        }

        private async Task OpenAddProductDialogAsync()
        {
            var parameters = new DialogParameters<AddProduct>();
            var dialog = await DialogService.ShowAsync<AddProduct>(null, parameters);
            var result = await dialog.Result;
            await OnInitializedAsync();
        }

        private void NavigatePage(string page) => NavigationManager.NavigateTo(page);

        private IEnumerable<UserProductDto> FilteredProducts =>
            SearchService.Search(SearchText, dto, b => [b.Serial_Number, b.ProductName]) ?? [];
        private void ShowBtnPress() => ShowDetails = !ShowDetails;

        private bool IsWarrantyValid(DateOnly date)
            => date > DateOnly.FromDateTime(DateTime.Now);
    }
}
