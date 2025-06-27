using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.ProductTypeDTOs;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;
using TechnicalService.Portal.Admin.Constants;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Extensions;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.Validations.Common.Validations.ProductTypeValidators;

namespace TechnicalService.Portal.Admin.Pages.ProductTypes
{
    public partial class UpdateProductType : ComponentBase
    {
        [Parameter] public int Id { get; set; }

        MudForm form;
        private UpdateProductTypeValidator Validator = new();
        private UpdateProductTypeDto updateProductTypeDto = new();
        private bool IsSubmitting { get; set; } = false;

        [Inject] private IDataService<Result<UpdateProductTypeDto>> DataService { get; set; }
        [Inject] private IDataService<UpdateProductTypeDto> UpdateProductTypeDataService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadProductTypeAsync();
        }

        private async Task LoadProductTypeAsync()
        {
            IsSubmitting = true;
            var response = await DataService.GetAsync(Endpoints.GetProductTypeById + Id, ClientTypes.PersonnelAuthClient);

            if (response.IsSuccess)
            {
                updateProductTypeDto = response.Data;
            }
            else if (response.StatusCode == StatusCode.Conflict)
            {
                NavigationOnSnackbar.NavigateOnClick(Snackbar, NavigationManager, response.StatusMessage, string.Format(AdminRouteConstants.AdminUpdateProductTypes, response.Data));
            }
            else
            {
                Snackbar.Add(response.StatusMessage, Severity.Error);
                NavigateBack();
            }
            IsSubmitting = false;
        }

        private async Task Submit()
        {
            await form.Validate();

            if (!form.IsValid)
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

        private async Task<Result<int>> SendData()
        {
            var result = await UpdateProductTypeDataService.UpdateAsync(Endpoints.UpdateProductType, updateProductTypeDto, ClientTypes.PersonnelAuthClient);
            return await result.Content.ReadFromJsonAsync<Result<int>>();
        }

        private void NavigateBack() => NavigationManager.NavigateTo(AdminRouteConstants.AdminProductTypes);
    }
} 