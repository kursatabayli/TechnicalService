using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.UserDTOs;
using TechnicalService.DTOs.Results;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.UserUI.Constants;
using TechnicalService.Validations.Common.Validations.UserValidators;

namespace TechnicalService.UserUI.Pages.Profile
{
    public partial class ChangePassword : ComponentBase
    {
        MudForm form;
        private ChangeUserPasswordDto dto = new();
        private ChangeUserPassowrdValidator Validator = new();
        private bool IsSubmitting = false;
        [Inject] private IDataService<ChangeUserPasswordDto> dataService { get; set; }
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

            var result = await dataService.UpdateAsync(Endpoints.UserChangePassword, dto, ClientTypes.UserAuthClient);
            return await result.Content.ReadFromJsonAsync<Result>();
        }

        private void NavigateBack() => NavigationManager.NavigateTo(UserRouteConstants.Profile);

    }
}
