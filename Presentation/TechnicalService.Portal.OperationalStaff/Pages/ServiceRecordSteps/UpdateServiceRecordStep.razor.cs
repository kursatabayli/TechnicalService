using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.ServiceRecordStepDTOs;
using TechnicalService.DTOs.Results;
using TechnicalService.RazorHelpers.Shared;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.Validations.Common.Validations.ServiceRecordValidators;

namespace TechnicalService.Portal.OperationalStaff.Pages.ServiceRecordSteps
{
    public partial class UpdateServiceRecordStep : ComponentBase
    {
        private MudForm form;
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
        [Parameter] public int StepId { get; set; }
        private UpdateServiceRecordStepDto UpdateServiceRecordStepDto { get; set; } = new();
        private UpdateServiceRecordStepValidator Validator { get; set; } = new();
        [Inject] private IDataService<UpdateServiceRecordStepDto> dataService { get; set; }
        [Inject] private IDataService<Result<UpdateServiceRecordStepDto>> DataService { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        private bool IsSubmitting { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadStepData();
            await DialogStyles();
        }
        private async Task LoadStepData()
        {
            var result = await DataService.GetAsync(Endpoints.GetServiceRecordStepById + StepId, ClientTypes.PersonnelAuthClient);
            if (result.IsSuccess)
                UpdateServiceRecordStepDto = result.Data;
            else
            {
                await OpenSimpleTextDialogAsync("Hata", "Servis adımı verileri yüklenemedi: " + result.StatusMessage, Icons.Material.Filled.Warning, Color.Warning);
                MudDialog.Cancel();
            }
        }
        private async Task Submit()
        {
            if (!await ValidateFormAsync())
                return;

            IsSubmitting = true;
            try
            {
                var result = await dataService.UpdateAsync(Endpoints.UpdateServiceRecordStep, UpdateServiceRecordStepDto, ClientTypes.PersonnelAuthClient);
                var response = await result.Content.ReadFromJsonAsync<Result>();
                Cancel();
                if (response.IsSuccess)
                    await OpenSimpleTextDialogAsync("Servis Adım Güncellemesi", response.StatusMessage, Icons.Material.Filled.CheckCircle, Color.Success);
                else
                    await OpenSimpleTextDialogAsync("Kayıt Hatası", response.StatusMessage, Icons.Material.Filled.Cancel, Color.Error);
            }
            catch (Exception ex)
            {
                Cancel();
                await OpenSimpleTextDialogAsync("Hata", "Teknik bir sorun oluştu: " + ex.Message, Icons.Material.Filled.Warning, Color.Warning);
            }
            finally
            {
                IsSubmitting = false;
            }

        }
        private async Task<bool> ValidateFormAsync()
        {
            await form.Validate();
            return form.IsValid;
        }
        private void Cancel() => MudDialog.Cancel();



        private Task DialogStyles()
        {
            var options = MudDialog.Options with
            {
                MaxWidth = MaxWidth.Small,
                FullWidth = true,
            };
            return MudDialog.SetOptionsAsync(options);
        }

        private async Task OpenSimpleTextDialogAsync(string title, string content, string contentIcon, Color generalColor)
        {

            var parameters = new DialogParameters<SimpleResponseDialog>
            {
                { x => x.Content,  content},
                { x => x.Title, title },
                { x => x.ContentIcon, contentIcon },
                { x => x.GeneralColor, generalColor },
            };

            var dialog = await DialogService.ShowAsync<SimpleResponseDialog>(null, parameters);
        }
    }
}
