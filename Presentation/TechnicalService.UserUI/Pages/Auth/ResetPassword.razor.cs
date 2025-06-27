using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.UserDTOs;
using TechnicalService.DTOs.Results;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.Validations.Common.Validations.UserValidators;

namespace TechnicalService.UserUI.Pages.Auth
{
    public partial class ResetPassword : ComponentBase
    {
        private MudForm form;
        private bool Submitting { get; set; } = true;
        private ResetUserPasswordDto dto = new();
        private ResetUserPasswordValidator Validator = new();
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private IDataService<ResetUserPasswordDto> dataService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Parameter] public string Token { get; set; }

        protected override void OnInitialized()
        {
            try
            {
                var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
                var queryParameters = QueryHelpers.ParseQuery(uri.Query);

                if (queryParameters.TryGetValue("confirm", out var tokenValues))
                    Token = tokenValues.First();
                else
                    Snackbar.Add("Geçersiz Doğrulama Linki!", Severity.Error);

                Submitting = false;

            }
            catch (Exception ex)
            {
                Snackbar.Add($"Bir hata oluştu: {ex.Message}", Severity.Error);
            }
        }
        private async Task Submit()
        {

            if (!(await ValidateFormAsync()))
                return;

            Submitting = true;
            await PerformResetPassword();
            Submitting = false;
        }
        private async Task<bool> ValidateFormAsync()
        {
            await form.Validate();
            return form.IsValid;
        }

        private async Task PerformResetPassword()
        {
            dto.Token = Token;
            var result = await dataService.CreateAsync(Endpoints.ResetPassword, dto, ClientTypes.UserPublicClient);
            var response = await result.Content.ReadFromJsonAsync<Result>();

            if (response.IsSuccess)
            {
                Snackbar.Add(response.StatusMessage, Severity.Success);
                NavigationManager.NavigateTo("/login");
            }
            else
            {
                Snackbar.Add(response.StatusMessage, Severity.Error);
            }
        }
    }
}
