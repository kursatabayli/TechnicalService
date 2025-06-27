using Microsoft.AspNetCore.Components;
using MudBlazor;
using TechnicalService.DTOs.DTOs.AuthDTOs;
using TechnicalService.DTOs.Enums;
using TechnicalService.Shared.AuthHelpers;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.Validations.Common.Validations.AuthValidators;

namespace TechnicalService.UserUI.Pages.Auth
{
    public partial class Login : ComponentBase
    {
        private MudForm form;
        private readonly LoginDto dto = new();
        private readonly LoginValidator Validator = new();
        private bool Submitting = false;
        private bool emailNotVerified = false;
        private string userEmail = string.Empty;
        [Inject] private CustomAuthStateProvider AuthStateProvider { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private IAuthService AuthService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                NavigateMainPage();
                return;
            }

            dto.Email = string.Empty;
            dto.Password = string.Empty;
        }

        private async Task Submit()
        {

            if (!await ValidateFormAsync())
                return;

            try
            {
                Submitting = true;
                await HandleLogin();
            }
            finally
            {
                Submitting = false;
            }
        }
        private async Task<bool> ValidateFormAsync()
        {
            await form.Validate();
            return form.IsValid;
        }

        private async Task HandleLogin()
        {
            var result = await AuthService.LoginAsync(dto, Endpoints.UserLogin, ClientTypes.UserPublicClient);
            if (result.IsSuccess)
            {
                AuthStateProvider.UpdateAuthState();
                NavigateMainPage();
            }
            else
            {
                if (result.StatusCode == StatusCode.EmailNotVerified)
                {
                    emailNotVerified = true;
                    userEmail = dto.Email;
                }
                else
                {
                    Snackbar.Add(result.StatusMessage, Severity.Error);
                }
            }
            Submitting = false;
        }

        private void NavigateMainPage() => NavigationManager.NavigateTo("/", true);
    }
}
