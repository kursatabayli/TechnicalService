using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.ProductDTOs;
using TechnicalService.DTOs.DTOs.SerialNumberDTOs;
using TechnicalService.DTOs.Results;
using TechnicalService.Portal.Admin.Constants;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.Validations.Common.Validations.SerialNumberValidators;

namespace TechnicalService.Portal.Admin.Pages.SerialNumbers
{
    public partial class UpdateSerialNumber : ComponentBase
    {
        [Parameter] public int Id { get; set; }

        private MudForm form;
        private UpdateSerialNumberValidator Validator = new();
        private UpdateSerialNumberDto UpdateSerialNumberDto;
        private List<ProductDto> AvailableProducts = [];
        private bool IsSubmitting { get; set; } = true;

        [Inject] private IDataService<Result<List<ProductDto>>> GetProductsDataService { get; set; }
        [Inject] private IDataService<UpdateSerialNumberDto> UpdateSerialNumberDataService { get; set; }
        [Inject] private IDataService<Result<UpdateSerialNumberDto>> DataService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private ISearchService SearchService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await LoadInitialDataAsync();
            }
            finally
            {
                IsSubmitting = false;
            }
        }

        private async Task LoadInitialDataAsync()
        {
            var productsResult = await GetProductsDataService.GetAsync(Endpoints.GetAllProducts, ClientTypes.PersonnelAuthClient);
            var serialNumberResult = await DataService.GetAsync(Endpoints.GetSerialNumberById + Id, ClientTypes.PersonnelAuthClient);

            if (!productsResult.IsSuccess || !serialNumberResult.IsSuccess)
            {
                Snackbar.Add(productsResult.StatusMessage ?? serialNumberResult.StatusMessage, Severity.Error);
                return;
            }

            AvailableProducts = productsResult.Data;
            UpdateSerialNumberDto = serialNumberResult.Data;
            UpdateSerialNumberDto.Product = AvailableProducts.FirstOrDefault(p => p.Id == UpdateSerialNumberDto.ProductId);
        }

        private async Task Submit()
        {
            if (!await ValidateFormAsync())
                return;

            IsSubmitting = true;

            var response = await SendData();
            if (response.IsSuccess)
            {
                Snackbar.Add(response.StatusMessage, Severity.Success);
                NavigateBack();
            }
            else
            {
                Snackbar.Add(response.StatusMessage, Severity.Error);
            }
            IsSubmitting = false;
        }

        private async Task<bool> ValidateFormAsync()
        {
            await form.Validate();
            return form.IsValid;
        }

        private async Task<Result<int>> SendData()
        {
            // ProductId'yi DTO içindeki Product nesnesinden alıyoruz.
            UpdateSerialNumberDto.ProductId = UpdateSerialNumberDto.Product.Id;

            var result = await UpdateSerialNumberDataService.UpdateAsync(Endpoints.UpdateSerialNumber, UpdateSerialNumberDto, ClientTypes.PersonnelAuthClient);
            return await result.Content.ReadFromJsonAsync<Result<int>>();
        }

        private Task<IEnumerable<ProductDto>> SearchProducts(string value, CancellationToken token)
        {
            return Task.FromResult(SearchService.Search(value, AvailableProducts, b => [b.ProductName, b.BrandName, b.Type]) ?? []);
        }

        private void NavigateBack() => NavigationManager.NavigateTo(AdminRouteConstants.AdminSerialNumbers);
    }
}
