using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.TechnicalServiceDTOs;
using TechnicalService.DTOs.Results;
using TechnicalService.Portal.Admin.Constants;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.Validations.Common.Validations.TechnicalServiceValidators;

namespace TechnicalService.Portal.Admin.Pages.TechnicalServices
{
    public partial class CreateTechnicalService : ComponentBase
    {
        private MudForm form;
        private CreateTechnicalServiceValidator Validator = new();
        private CreateTechnicalServiceDto CreateTechnicalServiceDto = new();
        private bool IsSubmitting { get; set; } = false;

        [Inject] private IDataService<CreateTechnicalServiceDto> CreateTechnicalServiceDataService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        private async Task Submit()
        {
            if (!await ValidateFormAsync())
                return;

            IsSubmitting = true;

            var response = await SendData();
            if (response.IsSuccess)
            {
                Snackbar.Add(response.StatusMessage, Severity.Success);
                NavigateBack();
            }
            else
            {
                Snackbar.Add(response.StatusMessage, Severity.Error);
            }
            IsSubmitting = false;
        }

        private async Task<bool> ValidateFormAsync()
        {
            await form.Validate();
            return form.IsValid;
        }

        private async Task<Result<int>> SendData()
        {
            var result = await CreateTechnicalServiceDataService.CreateAsync(Endpoints.CreateTechnicalService, CreateTechnicalServiceDto, ClientTypes.PersonnelAuthClient);
            return await result.Content.ReadFromJsonAsync<Result<int>>();
        }

        private void NavigateBack() => NavigationManager.NavigateTo(AdminRouteConstants.AdminTechnicalServices);
    }
}
