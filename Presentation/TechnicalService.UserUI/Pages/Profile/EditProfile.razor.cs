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
    public partial class EditProfile : ComponentBase
    {
        private MudForm form;
        private UpdateUserDto dto = new();
        private UpdateUserValidator Validator = new();
        private bool IsSubmitting { get; set; } = true;
        [Inject] private IDataService<Result<UpdateUserDto>> DataService { get; set; }
        [Inject] private IDataService<UpdateUserDto> UpdateUserDataService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }

        protected override async Task OnInitializedAsync()
        {

            try
            {
                await InitializeModel();

            }
            catch (Exception)
            {
                Snackbar.Add("Bir hata oluştu.", Severity.Error);
            }
            finally
            {
                IsSubmitting = false;
            }
        }

        private async Task InitializeModel()
        {
            var result = await DataService.GetAsync(Endpoints.GetUser, ClientTypes.UserAuthClient);
            if (result.IsSuccess)
            {
                dto = result.Data;
            }
            else
            {
                Snackbar.Add(result.StatusMessage, Severity.Error);
                NavigateBack();
            }
        }

        private async Task Submit()
        {
            if (!await ValidateFormAsync())
                return;

            IsSubmitting = true;

            try
            {
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
            }
            finally
            {
                IsSubmitting = false;
            }
        }

        private async Task<Result<Guid>> SendData()
        {
            var result = await UpdateUserDataService.UpdateAsync(Endpoints.UserBaseUrl, dto, ClientTypes.UserAuthClient);
            return await result.Content.ReadFromJsonAsync<Result<Guid>>();
        }
        private async Task<bool> ValidateFormAsync()
        {
            await form.Validate();
            return form.IsValid;
        }


        private void NavigateBack() => NavigationManager.NavigateTo(UserRouteConstants.Profile);
    }
}
