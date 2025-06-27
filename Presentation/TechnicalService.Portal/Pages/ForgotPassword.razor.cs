using Microsoft.AspNetCore.Components;
using MudBlazor;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.DTOs.Results;
using TechnicalService.DTOs.DTOs.PersonnelDTOs;
using System.Net.Http.Json;
using TechnicalService.Validations.Common.Validations.PersonnelValidators;

namespace TechnicalService.Portal.Pages
{
    public partial class ForgotPassword : ComponentBase
    {
        private MudForm form;
        private readonly PersonnelRequestPasswordResetLinkDto dto = new();
        private readonly RequestPersonnelPasswordResetValidator Validator = new();
        private bool Submitting = false;
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private IDataService<PersonnelRequestPasswordResetLinkDto> dataService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            dto.Email = string.Empty;
        }

        private async Task Submit()
        {

            if (!await ValidateFormAsync())
                return;

            try
            {
                Submitting = true;
                await SendRequest();
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

        private async Task SendRequest()
        {
            var result = await dataService.CreateAsync(Endpoints.PersonnelRequestPasswordReset, dto, ClientTypes.PersonnelPublicClient);
            var response = await result.Content.ReadFromJsonAsync<Result>();
            if (response.IsSuccess)
            {
                Snackbar.Add(response.StatusMessage, Severity.Success);

                NavigationManager.NavigateTo("/login");
            }
            else
                Snackbar.Add(response.StatusMessage, Severity.Error);

            Submitting = false;
        }

    }
}
