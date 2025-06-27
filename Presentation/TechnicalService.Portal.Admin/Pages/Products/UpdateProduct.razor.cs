using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.BrandDTOs;
using TechnicalService.DTOs.DTOs.ProductDTOs;
using TechnicalService.DTOs.DTOs.ProductTypeDTOs;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;
using TechnicalService.Portal.Admin.Constants;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Extensions;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.Validations.Common.Validations.ProductValidators;

namespace TechnicalService.Portal.Admin.Pages.Products
{
    public partial class UpdateProduct
    {
        [Parameter] public int Id { get; set; }

        private MudForm form;
        private UpdateProductValidator Validator = new();
        private UpdateProductDto UpdateProductDto = new();
        private List<ProductTypeDto> AvailableProductTypes = [];
        private List<BrandDto> AvaliableBrands = [];
        private bool IsSubmitting { get; set; } = true;
        [Inject] private IDataService<Result<List<BrandDto>>> GetBrandsDataService { get; set; }
        [Inject] private IDataService<Result<List<ProductTypeDto>>> GetProductTypsDataService { get; set; }
        [Inject] private IDataService<UpdateProductDto> UpdateProductDataService { get; set; }
        [Inject] private IDataService<Result<UpdateProductDto>> DataService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private ISearchService SearchService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await LoadBrandsAndProductTypes();
            }
            finally
            {
                IsSubmitting = false;
            }
        }

        private async Task LoadBrandsAndProductTypes()
        {
            var brandResult = await GetBrandsDataService.GetAsync(Endpoints.GetAllBrands, ClientTypes.PersonnelAuthClient);
            var productTypeResult = await GetProductTypsDataService.GetAsync(Endpoints.GetAllProductTypes, ClientTypes.PersonnelAuthClient);
            var productResult = await DataService.GetAsync(Endpoints.GetProductById + Id, ClientTypes.PersonnelAuthClient);
            if (!brandResult.IsSuccess || !productTypeResult.IsSuccess || !productResult.IsSuccess)
            {
                Snackbar.Add(brandResult.StatusMessage ?? productTypeResult.StatusMessage ?? productResult.StatusMessage, Severity.Error);
                NavigateBack();
            }

            AvaliableBrands = brandResult.Data;
            AvailableProductTypes = productTypeResult.Data;
            UpdateProductDto = productResult.Data;
            UpdateProductDto.Brand = AvaliableBrands.FirstOrDefault(b => b.Id == UpdateProductDto.BrandId);
            UpdateProductDto.ProductType = AvailableProductTypes.FirstOrDefault(b => b.Id == UpdateProductDto.ProductTypeId);
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
            {
                NavigationOnSnackbar.NavigateOnClick(Snackbar, NavigationManager, response.StatusMessage, string.Format(AdminRouteConstants.AdminUpdateProduct, response.Data));
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
            UpdateProductDto.BrandId = UpdateProductDto.Brand.Id;
            UpdateProductDto.ProductTypeId = UpdateProductDto.ProductType.Id;
            var result = await UpdateProductDataService.UpdateAsync(Endpoints.UpdateProduct, UpdateProductDto, ClientTypes.PersonnelAuthClient);
            return await result.Content.ReadFromJsonAsync<Result<int>>();
        }


        //private void OnBrandSelected(BrandDto brand) => CreateProductDto.Brand = brand;
        //private void OnProductTypeSelected(ProductTypeDto productType) => CreateProductDto.ProductType = productType;

        private async Task<IEnumerable<BrandDto>> SearchBrands(string value, CancellationToken token)
            => await SearchService.SearchAsync(value, AvaliableBrands, b => b.BrandName, token);

        private async Task<IEnumerable<ProductTypeDto>> SearchProductTypes(string value, CancellationToken token)
            => await SearchService.SearchAsync(value, AvailableProductTypes, pt => pt.Type, token);

        private void NavigateBack() => NavigationManager.NavigateTo(AdminRouteConstants.AdminProducts);

    }
}