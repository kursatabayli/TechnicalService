using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.BrandDTOs;
using TechnicalService.Portal.Admin.Constants;
using TechnicalService.DTOs.Results;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Extensions;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.Validations.Common.Validations.BrandValidators;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.Portal.Admin.Pages.Brands
{
    public partial class CreateBrand : ComponentBase
    {

        MudForm form;
        private CreateBrandValidator Validator = new();
        private CreateBrandDto createBrandDto = new();
        private bool IsSubmitting { get; set; } = false;
        [Inject] private IDataService<CreateBrandDto> DataService { get; set; }
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
                NavigationOnSnackbar.NavigateOnClick(Snackbar, NavigationManager, response.StatusMessage, string.Format(AdminRouteConstants.AdminUpdateBrands, response.Data));
            }
            else
            {
                Snackbar.Add(response.StatusMessage, Severity.Error);
            }
            IsSubmitting = false;
        }

        private async Task<Result<int>> SendData()
        {
            var result = await DataService.CreateAsync(Endpoints.CreateBrand, createBrandDto, ClientTypes.PersonnelAuthClient);
            var response = await result.Content.ReadFromJsonAsync<Result<int>>();
            return response;
        }

        private void NavigateBack() => NavigationManager.NavigateTo(AdminRouteConstants.AdminBrands);
    }
}
