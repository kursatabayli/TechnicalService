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
    public partial class UpdateTechnicalService : ComponentBase
    {
        [Parameter] public int Id { get; set; }

        private MudForm form;
        private UpdateTechnicalServiceValidator Validator = new();
        private UpdateTechnicalServiceDto UpdateTechnicalServiceDto;
        private bool IsSubmitting { get; set; } = true;

        [Inject] private IDataService<UpdateTechnicalServiceDto> UpdateTechnicalServiceDataService { get; set; }
        [Inject] private IDataService<Result<UpdateTechnicalServiceDto>> DataService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await LoadTechnicalServiceAsync();
            }
            finally
            {
                IsSubmitting = false;
            }
        }

        private async Task LoadTechnicalServiceAsync()
        {
            var result = await DataService.GetAsync(Endpoints.GetTechnicalServiceById + Id, ClientTypes.PersonnelAuthClient);

            if (result.IsSuccess)
            {
                UpdateTechnicalServiceDto = result.Data;
            }
            else
            {
                Snackbar.Add(result.StatusMessage, Severity.Error);
            }
        }

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
            var result = await UpdateTechnicalServiceDataService.UpdateAsync(Endpoints.UpdateTechnicalService, UpdateTechnicalServiceDto, ClientTypes.PersonnelAuthClient);
            return await result.Content.ReadFromJsonAsync<Result<int>>();
        }

        private void NavigateBack() => NavigationManager.NavigateTo(AdminRouteConstants.AdminTechnicalServices);
    }
}
