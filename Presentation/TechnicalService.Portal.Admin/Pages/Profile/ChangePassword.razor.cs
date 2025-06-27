using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.PersonnelDTOs;
using TechnicalService.DTOs.Results;
using TechnicalService.Portal.Admin.Constants;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.Validations.Common.Validations.PersonnelValidators;

namespace TechnicalService.Portal.Admin.Pages.Profile
{
    public partial class ChangePassword : ComponentBase
    {
        MudForm form;
        private ChangePersonnelPasswordDto dto = new();
        private ChangePersonnelPassowrdValidator Validator = new();
        private bool IsSubmitting = false;
        [Inject] private IDataService<ChangePersonnelPasswordDto> dataService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }

        private async Task Submit()
        {

            if (!(await ValidateFormAsync()))
                return;

            var response = await ChangePasswordAsync();
            if (response.IsSuccess)
            {
                Snackbar.Add(response.StatusMessage, Severity.Success);
                NavigateBack();
            }
            else
            {
                Snackbar.Add(response.StatusMessage, Severity.Error);
            }
        }

        private async Task<bool> ValidateFormAsync()
        {
            await form.Validate();
            return form.IsValid;
        }
        private async Task<Result> ChangePasswordAsync()
        {

            var result = await dataService.UpdateAsync(Endpoints.PersonnelChangePassword, dto, ClientTypes.PersonnelAuthClient);
            return await result.Content.ReadFromJsonAsync<Result>();
        }

        private void NavigateBack() => NavigationManager.NavigateTo(AdminRouteConstants.AdminProfile);

    }
}
