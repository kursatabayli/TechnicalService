using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.PersonnelDTOs;
using TechnicalService.DTOs.DTOs.ServiceRecordDTOs;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;
using TechnicalService.RazorHelpers.Shared;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Extensions;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.Portal.Admin.Pages.ServiceRecords
{
    public partial class UpdateServiceRecord : ComponentBase
    {
        private MudForm form;
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
        [Parameter] public Guid ServiceRecordId { get; set; }
        private List<PersonnelDto> Personnels = [];
        private PersonnelDto SelectedPersonnel { get; set; } = new();
        private UpdateServiceRecordDto UpdateServiceRecordDto { get; set; } = new();
        [Inject] private IDataService<UpdateServiceRecordDto> UpdateServiceRecordData { get; set; }
        [Inject] private IDataService<Result<UpdateServiceRecordDto>> GetServiceRecordData { get; set; }
        [Inject] private IDataService<Result<List<PersonnelDto>>> GetPersonnels { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        private bool IsSubmitting { get; set; } = false;
        [Inject] private ISearchService SearchService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
            await DialogStyles();
        }
        private async Task LoadData()
        {
            var serviceRecordResult = await GetServiceRecordData.GetAsync(Endpoints.GetServiceRecordDetail + ServiceRecordId, ClientTypes.PersonnelAuthClient);
            if (serviceRecordResult.IsSuccess)
                UpdateServiceRecordDto = serviceRecordResult.Data;
            else
            {
                await OpenSimpleTextDialogAsync("Hata", "Güncelleme sırasında bir hata meydana geldi: " + serviceRecordResult.StatusMessage, Icons.Material.Filled.Warning, Color.Warning);
                MudDialog.Cancel();
            }
            var personnelsResult = await GetPersonnels.GetAsync(Endpoints.GetAllPersonnels, ClientTypes.PersonnelAuthClient);
            if (personnelsResult.IsSuccess)
            {
                Personnels = [.. personnelsResult.Data.OrderBy(s => s.ServiceName).GroupBy(r => r.Role).SelectMany(group => group.ToList())];
                SelectedPersonnel = Personnels.FirstOrDefault(p => p.Id == UpdateServiceRecordDto.PersonnelId);
            }
            else
                Snackbar.Add(personnelsResult.StatusMessage, Severity.Error);
        }
        private async Task Submit()
        {
            if (!await ValidateFormAsync())
                return;

            IsSubmitting = true;
            try
            {
                await SendData();
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
        private async Task SendData()
        {
            UpdateServiceRecordDto.PersonnelId = SelectedPersonnel?.Id;

            var result = await UpdateServiceRecordData.UpdateAsync(Endpoints.UpdateServiceRecord, UpdateServiceRecordDto, ClientTypes.PersonnelAuthClient);
            var response = await result.Content.ReadFromJsonAsync<Result>();
            Cancel();
            if (response.IsSuccess)
                await OpenSimpleTextDialogAsync("Servis Kaydı Güncellendi", response.StatusMessage, Icons.Material.Filled.CheckCircle, Color.Success);
            else
                await OpenSimpleTextDialogAsync("Kayıt Hatası", response.StatusMessage, Icons.Material.Filled.Cancel, Color.Error);


        }
        private async Task<IEnumerable<PersonnelDto>> SearchPersonnel(string value, CancellationToken token)
            => await SearchService.SearchAsync(value, Personnels, b => $"{b.Name} {b.Surname} {b.ServiceName}", token);

        private async Task<bool> ValidateFormAsync()
        {
            await form.Validate();
            return form.IsValid;
        }
        private void Cancel() => MudDialog.Cancel();

        private bool IsPersonnelDisabled(PersonnelDto personnel)
        {
            if (personnel == null)
                return true;

            return !(personnel.PersonnelStatus == PersonnelStatusDto.Active);
        }

        private string StringFunc(PersonnelDto personnel)
        {
            if (personnel == null)
                return string.Empty;
            if (personnel.PersonnelStatus == PersonnelStatusDto.Active)
                return $"{personnel.Name} {personnel.Surname} / {personnel.Role.GetDescription()} - {personnel.ServiceName}";
            else
                return $"{personnel.Name} {personnel.Surname} / {personnel.Role.GetDescription()} - {personnel.ServiceName} ({personnel.PersonnelStatus.GetDescription()})";
        }

        private string GetStatusIcon()
        {
            return UpdateServiceRecordDto.Status switch
            {
                ServiceStatusDto.Pending => Icons.Material.Filled.HourglassTop,
                ServiceStatusDto.InProgress => Icons.Material.Filled.Build,
                ServiceStatusDto.Completed => Icons.Material.Filled.CheckCircle,
                _ => Icons.Material.Filled.HelpOutline
            };
        }

        private Color GetStatusColor()
        {
            return UpdateServiceRecordDto.Status switch
            {
                ServiceStatusDto.Pending => Color.Warning,
                ServiceStatusDto.InProgress => Color.Info,
                ServiceStatusDto.Completed => Color.Success,
                _ => Color.Default
            };
        }
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
