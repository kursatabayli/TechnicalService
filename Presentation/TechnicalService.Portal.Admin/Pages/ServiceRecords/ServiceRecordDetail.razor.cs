using Microsoft.AspNetCore.Components;
using MudBlazor;
using TechnicalService.DTOs.DTOs.ServiceRecordDTOs;
using TechnicalService.DTOs.DTOs.ServiceRecordStepDTOs;
using TechnicalService.DTOs.DTOs.UserDTOs;
using TechnicalService.DTOs.DTOs.UserProductDTOs;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;
using TechnicalService.Portal.Admin.Pages.ServiceRecordSteps;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.Portal.Admin.Pages.ServiceRecords
{
    public partial class ServiceRecordDetail : ComponentBase
    {
        [Parameter] public Guid Id { get; set; }
        private bool _isLoading = true;
        private ServiceRecordDto _serviceRecordDto;
        private UserProductDto _userProductDto;
        private UserDto _userDetailDto;
        private List<ServiceRecordStepDto> _serviceRecordSteps;

        [Inject] private IDataService<Result<ServiceRecordDto>> ServiceRecordDataService { get; set; }
        [Inject] private IDataService<Result<UserProductDto>> UserProductDataService { get; set; }
        [Inject] private IDataService<Result<UserDto>> UserDataService { get; set; }
        [Inject] private IDataService<Result<List<ServiceRecordStepDto>>> ServiceRecordStepDataService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private IDialogService DialogService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadPageAsync();
        }

        private async Task LoadPageAsync()
        {
            try
            {
                await LoadServiceRecord();
                await LoadServiceRecordSteps();
                await LoadUserDetails();
                await LoadUserProductDetail();
            }
            catch (Exception)
            {
                Snackbar.Add("Beklenmedik bir hata oluştu", Severity.Error);
            }
            finally
            {
                _isLoading = false;
                StateHasChanged();
            }
        }

        private async Task LoadServiceRecord()
        {
            var serviceRecordResult = await ServiceRecordDataService.GetAsync(Endpoints.GetServiceRecordDetail + Id, ClientTypes.PersonnelAuthClient);
            if (serviceRecordResult.IsSuccess && serviceRecordResult.Data != null)
                _serviceRecordDto = serviceRecordResult.Data;
            else
                Snackbar.Add(serviceRecordResult.StatusMessage, Severity.Error);
        }
        public async Task LoadServiceRecordSteps()
        {
            var serviceRecordStepsResult = await ServiceRecordStepDataService.GetAsync(Endpoints.GetServiceRecordStepsByServiceRecordId + Id, ClientTypes.PersonnelAuthClient);
            if (serviceRecordStepsResult.IsSuccess && serviceRecordStepsResult.Data != null)
                _serviceRecordSteps = [.. serviceRecordStepsResult.Data];
            else
                Snackbar.Add(serviceRecordStepsResult.StatusMessage, Severity.Error);

            StateHasChanged();
        }
        private async Task LoadUserDetails()
        {
            var userDetailResult = await UserDataService.GetAsync(Endpoints.GetUserById + _serviceRecordDto.UserId, ClientTypes.PersonnelAuthClient);
            if (userDetailResult.IsSuccess && userDetailResult.Data != null)
                _userDetailDto = userDetailResult.Data;
            else
                Snackbar.Add(userDetailResult.StatusMessage, Severity.Error);
        }
        private async Task LoadUserProductDetail()
        {
            var userProductResult = await UserProductDataService.GetAsync(Endpoints.GetUserProductById + _serviceRecordDto.UserProductId, ClientTypes.PersonnelAuthClient);
            if (userProductResult.IsSuccess && userProductResult.Data != null)
                _userProductDto = userProductResult.Data;
            else
                Snackbar.Add(userProductResult.StatusMessage, Severity.Error);
        }

        private async Task OpenAddServiceStepDialogAsync()
        {
            var parameters = new DialogParameters<AddServiceRecordStep>
            {
                { nameof(AddServiceRecordStep.ServiceRecordId), _serviceRecordDto.Id },
                { nameof(AddServiceRecordStep.Order), _serviceRecordSteps.Count + 1 }
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true };
            var dialog = await DialogService.ShowAsync<AddServiceRecordStep>("Yeni Servis Adımı Ekle", parameters, options);
            var result = await dialog.Result;

            await LoadServiceRecordSteps();
        }
        private async Task OpenServiceRecordUpdateDialogAsync()
        {
            var parameters = new DialogParameters<UpdateServiceRecord>
            {
                { nameof(UpdateServiceRecord.ServiceRecordId), Id },
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true };
            var dialog = await DialogService.ShowAsync<UpdateServiceRecord>("Servis Kaydını Güncelle", parameters, options);
            var result = await dialog.Result;

            await LoadServiceRecord();
        }

        private Color GetStatusColor(ServiceStatusDto status) => status switch
        {
            ServiceStatusDto.Pending => Color.Warning,
            ServiceStatusDto.InProgress => Color.Info,
            ServiceStatusDto.Completed => Color.Success,
            _ => Color.Default
        };
    }
}
