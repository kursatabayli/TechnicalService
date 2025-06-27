using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.BrandDTOs;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;
using TechnicalService.Portal.Admin.Constants;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Extensions;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.Validations.Common.Validations.BrandValidators;

namespace TechnicalService.Portal.Admin.Pages.Brands
{
    public partial class UpdateBrand : ComponentBase
    {
        [Parameter] public int Id { get; set; }

        MudForm form;
        private UpdateBrandValidator Validator = new();
        private UpdateBrandDto updateBrandDto = new();
        private bool IsSubmitting { get; set; } = false;

        [Inject] private IDataService<Result<UpdateBrandDto>> DataService { get; set; }
        [Inject] private IDataService<UpdateBrandDto> UpdateBrandDataService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadBrandAsync();
        }

        private async Task LoadBrandAsync()
        {
            IsSubmitting = true;
            var response = await DataService.GetAsync(Endpoints.GetBrandById + Id, ClientTypes.PersonnelAuthClient);

            if (response.IsSuccess)
            {
                updateBrandDto = response.Data;
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
            else if (response.StatusCode == StatusCode.Conflict)
            {
                NavigationOnSnackbar.NavigateOnClick(Snackbar, NavigationManager, response.StatusMessage, string.Format(AdminRouteConstants.AdminUpdateBrands, response.Data));
            }
            else
            {
                Snackbar.Add(response.StatusMessage, Severity.Error);
            }

        }

        private async Task<Result<int>> SendData()
        {
            var result = await UpdateBrandDataService.UpdateAsync(Endpoints.UpdateBrand, updateBrandDto, ClientTypes.PersonnelAuthClient);
            return await result.Content.ReadFromJsonAsync<Result<int>>();
        }

        private void NavigateBack() => NavigationManager.NavigateTo(AdminRouteConstants.AdminBrands);
    }
}