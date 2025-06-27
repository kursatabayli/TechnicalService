using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.ServiceRecordDTOs;
using TechnicalService.DTOs.DTOs.UserProductDTOs;
using TechnicalService.DTOs.Results;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.UserUI.Constants;
using TechnicalService.Validations.Common.Validations.ServiceRecordValidators;

namespace TechnicalService.UserUI.Pages.ServiceRecord
{
    public partial class CreateServiceRecord : ComponentBase
    {
        private MudForm form;
        private bool IsSubmitting = false;
        private CreateServiceRecordDto dto = new();
        private CreateServiceRecordValidator Validator = new();
        private List<UserProductDto> userProducts = new();
        [Inject] private IDataService<CreateServiceRecordDto> DataService { get; set; }
        [Inject] private IDataService<Result<List<UserProductDto>>> GetUserProductDataService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private ISearchService SearchService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadUserProducts();
        }
        private async Task LoadUserProducts()
        {
            var result = await GetUserProductDataService.GetAsync(Endpoints.GetUserProducts, ClientTypes.UserAuthClient);
            if (result.IsSuccess)
                userProducts = result.Data;
            else
            {
                Snackbar.Add(result.StatusMessage, Severity.Error);
                NavigatePage(UserRouteConstants.ServiceRecords);
            }
        }

        private async Task Submit()
        {
            if (!await ValidateFormAsync())
                return;
            try
            {
                IsSubmitting = true;
                var response = await SendData();
                if (response.IsSuccess)
                {
                    Snackbar.Add(response.StatusMessage, Severity.Success);
                    NavigatePage(string.Format(UserRouteConstants.ServiceRecordStatus, response.Data));
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
        private async Task<bool> ValidateFormAsync()
        {
            await form.Validate();
            return form.IsValid;
        }
        private async Task<Result<Guid>> SendData()
        {
            dto.UserProductId = dto.UserProduct.Id;
            var result = await DataService.CreateAsync(Endpoints.CreateServiceRecord, dto, ClientTypes.UserAuthClient);
            return await result.Content.ReadFromJsonAsync<Result<Guid>>();
        }

        private Task<IEnumerable<UserProductDto>> SearchProduct(string value, CancellationToken token)
            => Task.FromResult(SearchService.Search(value, userProducts, b => [b.ProductName, b.Serial_Number]) ?? []);

        private void NavigatePage(string page) => NavigationManager.NavigateTo(page);

    }
}
