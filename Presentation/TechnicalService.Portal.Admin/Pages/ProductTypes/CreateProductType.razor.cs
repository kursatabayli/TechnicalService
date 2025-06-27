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
    public partial class CreateProductType : ComponentBase
    {
        MudForm form;
        private CreateProductTypeValidator Validator = new();
        private CreateProductTypeDto createProductTypeDto = new();
        private bool IsSubmitting { get; set; } = false;

        [Inject] private IDataService<CreateProductTypeDto> DataService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }

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
            else if (response.StatusCode == StatusCode.Conflict)
            {
                NavigationOnSnackbar.NavigateOnClick(Snackbar, NavigationManager, response.StatusMessage, string.Format(AdminRouteConstants.AdminUpdateProductTypes, response.Data));
            }
            else
            {
                Snackbar.Add(response.StatusMessage, Severity.Error);
            }
            IsSubmitting = false;
        }

        private async Task<Result<int>> SendData()
        {
            var result = await DataService.CreateAsync(Endpoints.CreateProductType, createProductTypeDto, ClientTypes.PersonnelAuthClient);
            return await result.Content.ReadFromJsonAsync<Result<int>>();
        }

        private void NavigateBack() => NavigationManager.NavigateTo(AdminRouteConstants.AdminProductTypes);
    }
}