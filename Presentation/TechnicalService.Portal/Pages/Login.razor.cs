using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Security.Claims;
using TechnicalService.DTOs.DTOs.AuthDTOs;
using TechnicalService.DTOs.Enums;
using TechnicalService.Portal.Constants;
using TechnicalService.Shared.AuthHelpers;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Extensions;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.Validations.Common.Validations.AuthValidators;

namespace TechnicalService.Portal.Pages
{
    public partial class Login : ComponentBase
    {
        private MudForm form;
        private readonly LoginDto dto = new();
        private readonly LoginValidator Validator = new();
        private bool Submitting = false;
        [Inject] private CustomAuthStateProvider AuthStateProvider { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private IAuthService AuthService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await NavigateMainPage();
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
            var result = await AuthService.LoginAsync(dto, Endpoints.PersonnelLogin, ClientTypes.PersonnelPublicClient);
            if (result.IsSuccess)
            {
                AuthStateProvider.UpdateAuthState();
                await NavigateMainPage();
            }
            else
                Snackbar.Add(result.StatusMessage, Severity.Error);

            Submitting = false;
        }

        private async Task NavigateMainPage()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var userClaims = authState.User.Claims;

            if (!userClaims.Any())
                return;

            var userRole = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (userRole == RoleDto.Admin.GetDescription())
                NavigationManager.NavigateTo(RouteConstants.admin, true);
            else if (userRole == RoleDto.Manager.GetDescription())
                NavigationManager.NavigateTo(RouteConstants.managment, true);
            else if (userRole == RoleDto.Technician.GetDescription() || userRole == RoleDto.CustomerService.GetDescription())
                NavigationManager.NavigateTo(RouteConstants.operational, true);
        }
    }
}
