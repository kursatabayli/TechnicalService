using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net;
using TechnicalService.DTOs.DTOs.AuthDTOs;
using TechnicalService.DTOs.DTOs.LegalDocumentDTOs;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;
using TechnicalService.RazorHelpers.Shared;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.Validations.Common.Validations.AuthValidators;

namespace TechnicalService.UserUI.Pages.Auth
{
    public partial class Register : ComponentBase
    {
        MudForm form;
        private RegisterDto dto = new();
        private RegisterUserValidator Validator = new();
        private bool TermsOfService = false;
        private bool PrivacyAndPdplPolicy = false;
        private bool Submitting = false;
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private IAuthService AuthService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        [Inject] private IDataService<Result<LegalDocumentDto>> GetLegalDocumentDataService { get; set; }

        private async Task Submit()
        {
            if (!await ValidateFormAsync() || !TermsOfService)
                return;

            try
            {
                Submitting = true;
                await HandleRegistration();
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

        private async Task HandleRegistration()
        {
            var result = await AuthService.RegisterUserAsync(dto, ClientTypes.UserPublicClient);
            if (result.IsSuccess)
            {
                NavigationManager.NavigateTo("/login", true);
                Snackbar.Add(result.StatusMessage, Severity.Success);
            }
            else
            {
                Snackbar.Add(result.StatusMessage, Severity.Error);
            }
        }

        private async Task ShowTermsOfServiceAsync()
        {
            var legalDocument = await GetLegalDocumentDataService.GetAsync(Endpoints.GetLegalDocumentByDocumentType + ((int)DocumentTypeDto.TermsOfService), ClientTypes.UserPublicClient);
            var parameters = new DialogParameters<SimpleDialog>
                {
                    { x => x.LicenseText, legalDocument.Data.Content },
                };
            var dialog = await DialogService.ShowAsync<SimpleDialog>(null, parameters);
            await dialog.Result;
        }
        private async Task ShowPrivacyAndPdplPolicyAsync()
        {
            var legalDocument = await GetLegalDocumentDataService.GetAsync(Endpoints.GetLegalDocumentByDocumentType + ((int)DocumentTypeDto.PrivacyAndPdplPolicy), ClientTypes.UserPublicClient);
            var parameters = new DialogParameters<SimpleDialog>
                {
                    { x => x.LicenseText, legalDocument.Data.Content },
                };
            var dialog = await DialogService.ShowAsync<SimpleDialog>(null, parameters);
            await dialog.Result;
        }
    }
}
