using Microsoft.AspNetCore.Components;
using MudBlazor;
using TechnicalService.DTOs.DTOs.AuthDTOs;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.DTOs.Results;

namespace TechnicalService.UserUI.Pages.Auth
{
    public partial class ForgotPassword : ComponentBase
    {
        private MudForm form;
        private readonly RequestResetPasswordLinkDto dto = new();
        private bool Submitting = false;
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private IDataService<Result> dataService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            dto.Email = string.Empty;
        }

        private async Task Submit()
        {

            if (!(await ValidateFormAsync()))
                return;

            try
            {
                Submitting = true;
                await SendRequest();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Hata: {ex.Message}", Severity.Error);
                Submitting = false;
            }
        }
        private async Task<bool> ValidateFormAsync()
        {
            await form.Validate();
            return form.IsValid;
        }

        private async Task SendRequest()
        {
            var result = await dataService.GetAsync(Endpoints.UserRequestPasswordReset + dto.Email, ClientTypes.UserPublicClient);

            if (result.IsSuccess)
            {
                Snackbar.Add(result.StatusMessage, Severity.Success);

                NavigationManager.NavigateTo("/login");
            }
            else
                Snackbar.Add(result.StatusMessage, Severity.Error);

            Submitting = false;
        }

    }
}
