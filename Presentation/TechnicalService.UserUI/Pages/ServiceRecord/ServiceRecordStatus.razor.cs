using Microsoft.AspNetCore.Components;
using MudBlazor;
using TechnicalService.DTOs.DTOs.ServiceRecordDTOs;
using TechnicalService.DTOs.DTOs.ServiceRecordStepDTOs;
using TechnicalService.DTOs.DTOs.UserProductDTOs;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.UserUI.Pages.ServiceRecord
{
    public partial class ServiceRecordStatus : ComponentBase
    {
        [Parameter] public Guid Id { get; set; }
        private bool LoadingData { get; set; } = true;
        private List<ServiceRecordStepDto> serviceRecordSteps { get; set; } = [];
        private ServiceRecordDto ServiceRecordDto { get; set; } = new();
        private UserProductDto UserProduct { get; set; } = new();
        [Inject] private IDataService<Result<List<ServiceRecordStepDto>>> stepsDataService { get; set; }
        [Inject] private IDataService<Result<ServiceRecordDto>> requestDataService { get; set; }
        [Inject] private IDataService<Result<UserProductDto>> userProductDataService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }

        protected override async Task OnInitializedAsync()
        {

            await LoadRequestDetails();
        }

        private async Task LoadRequestDetails()
        {
            try
            {
                var repairRequest = await requestDataService.GetAsync(Endpoints.GetServiceRecordById + Id, ClientTypes.UserAuthClient);
                if (repairRequest.IsSuccess)
                    ServiceRecordDto = repairRequest.Data;
                else
                {
                    Snackbar.Add(repairRequest.StatusMessage, Severity.Error);
                    return;
                }

                var userProduct = await userProductDataService.GetAsync(Endpoints.GetUserProductByUserProductId + ServiceRecordDto.UserProductId, ClientTypes.UserAuthClient);

                if (userProduct.IsSuccess)
                    UserProduct = userProduct.Data;
                else
                {
                    Snackbar.Add(userProduct.StatusMessage, Severity.Error);
                    return;
                }

                var steps = await stepsDataService.GetAsync(Endpoints.GetServiceRecordStepsByServiceRecordId + Id, ClientTypes.UserAuthClient);

                if (steps.IsSuccess)
                    serviceRecordSteps = steps.Data;
                else
                {
                    Snackbar.Add(steps.StatusMessage, Severity.Error);
                    return;
                }
            }
            catch (Exception)
            {
                Snackbar.Add("Bir hata meydana geldi.", Severity.Error);
            }
            finally
            {
                LoadingData = false;
            }

        }

        private Color GetStatusColor(ServiceStatusDto status)
            => status switch
            {
                ServiceStatusDto.Pending => Color.Inherit,
                ServiceStatusDto.InProgress => Color.Warning,
                ServiceStatusDto.Completed => Color.Success,
            };
    }
}
