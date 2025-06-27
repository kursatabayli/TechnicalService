using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.ProductDTOs;
using TechnicalService.DTOs.DTOs.SerialNumberDTOs;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;
using TechnicalService.Portal.Admin.Constants;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Extensions;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.Validations.Common.Validations.SerialNumberValidators;

namespace TechnicalService.Portal.Admin.Pages.SerialNumbers
{
    public partial class CreateSerialNumber : ComponentBase
    {
        private MudForm form;
        private CreateSerialNumberValidator Validator = new();
        private CreateSerialNumberDto CreateSerialNumberDto = new();
        private List<ProductDto> AvailableProducts = [];
        private bool IsSubmitting { get; set; } = false;

        [Inject] private IDataService<Result<List<ProductDto>>> GetProductsDataService { get; set; }
        [Inject] private IDataService<CreateSerialNumberDto> CreateSerialNumberDataService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private ISearchService SearchService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadProductsAsync();
        }

        private async Task LoadProductsAsync()
        {
            var productResult = await GetProductsDataService.GetAsync(Endpoints.GetAllProducts, ClientTypes.PersonnelAuthClient);

            if (!productResult.IsSuccess)
            {
                Snackbar.Add(productResult.StatusMessage, Severity.Error);
                NavigateBack();
            }

            AvailableProducts = productResult.Data;
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
            else if (response.StatusCode == StatusCode.Conflict)
                NavigationOnSnackbar.NavigateOnClick(Snackbar, NavigationManager, response.StatusMessage, string.Format(AdminRouteConstants.AdminUpdateSerialNumbers, response.Data));
            else
                Snackbar.Add(response.StatusMessage, Severity.Error);
            IsSubmitting = false;
        }

        private async Task<bool> ValidateFormAsync()
        {
            await form.Validate();
            return form.IsValid;
        }

        private async Task<Result<int>> SendData()
        {
            CreateSerialNumberDto.ProductId = CreateSerialNumberDto.Product.Id;
            var result = await CreateSerialNumberDataService.CreateAsync(Endpoints.CreateSerialNumber, CreateSerialNumberDto, ClientTypes.PersonnelAuthClient);
            return await result.Content.ReadFromJsonAsync<Result<int>>();
        }

        private async Task<IEnumerable<ProductDto>> SearchProducts(string value, CancellationToken token)
        {
            return await SearchService.SearchAsync(value, AvailableProducts, p => p.ProductName, token);
        }

        private void NavigateBack() => NavigationManager.NavigateTo(AdminRouteConstants.AdminSerialNumbers);
    }
}
