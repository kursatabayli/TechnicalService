using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.PersonnelDTOs;
using TechnicalService.DTOs.DTOs.TechnicalServiceDTOs;
using TechnicalService.Portal.Admin.Constants;
using TechnicalService.DTOs.Results;
using TechnicalService.DTOs.Enums;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Extensions;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.Validations.Common.Validations.PersonnelValidators;

namespace TechnicalService.Portal.Admin.Pages.Personnels
{
    public partial class CreatePersonnel : ComponentBase
    {
        private MudForm form;
        private CreatePersonnelValidator Validator = new();
        private CreatePersonnelDto CreatePersonnelDto = new();
        private bool IsSubmitting { get; set; } = false;
        private List<TechnicalServiceDto> TechnicalServices = [];
        private IEnumerable<RoleDto> Roles { get; set; }
        [Inject] private IDataService<CreatePersonnelDto> CreatePersonnelDataService { get; set; }
        [Inject] private IDataService<Result<List<TechnicalServiceDto>>> GetTechnicalServicesDataService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private ISearchService SearchService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadTechnicalServicesAsync();
        }
        private async Task LoadTechnicalServicesAsync()
        {
            var result = await GetTechnicalServicesDataService.GetAsync(Endpoints.GetAllTechnicalServices, ClientTypes.PersonnelPublicClient);
            if (result.IsSuccess)
            {
                TechnicalServices = result.Data;
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

            var response = await SendData();
            if (response.IsSuccess)
            {
                Snackbar.Add(response.StatusMessage, Severity.Success);
                NavigateBack();
            }
            else if (response.StatusCode == StatusCode.Conflict)
            {
                NavigationOnSnackbar.NavigateOnClick(Snackbar, NavigationManager, response.StatusMessage, string.Format(AdminRouteConstants.AdminUpdatePersonnel, response.Data));
            }
            else
            {
                Snackbar.Add(response.StatusMessage, Severity.Error);
            }
            IsSubmitting = false;
        }


        private async Task<Result<Guid>> SendData()
        {
            CreatePersonnelDto.TechnicalServiceId = CreatePersonnelDto.TechnicalServices.Id;
            var result = await CreatePersonnelDataService.CreateAsync(Endpoints.CreatePersonnel, CreatePersonnelDto, ClientTypes.PersonnelAuthClient);
            return await result.Content.ReadFromJsonAsync<Result<Guid>>();
        }

        private async Task<bool> ValidateFormAsync()
        {
            await form.Validate();
            return form.IsValid;
        }

        private async Task<IEnumerable<TechnicalServiceDto>> SearchTechnicalService(string value, CancellationToken token)
            => await SearchService.SearchAsync(value, TechnicalServices, b => b.ServiceName, token);


        protected override void OnInitialized()
        {
            Roles = [.. Enum.GetValues<RoleDto>()
                                .Where(r => r != RoleDto.User)];
            base.OnInitialized();
        }
        private string StatusIcon(PersonnelStatusDto status)
        {
            return status switch
            {
                PersonnelStatusDto.Active => Icons.Material.Filled.CheckCircle,
                PersonnelStatusDto.OnLeave => Icons.Material.Filled.EventBusy,
                PersonnelStatusDto.Terminated => Icons.Material.Filled.PersonOff,
                PersonnelStatusDto.Suspended => Icons.Material.Filled.Block,
                _ => Icons.Material.Filled.HelpOutline
            };
        }
        private Color StatusColor(PersonnelStatusDto status)
        {
            return status switch
            {
                PersonnelStatusDto.Active => Color.Success,
                PersonnelStatusDto.OnLeave => Color.Warning,
                PersonnelStatusDto.Terminated => Color.Error,
                PersonnelStatusDto.Suspended => Color.Info,
                _ => Color.Default
            };
        }
        private void NavigateBack() => NavigationManager.NavigateTo(AdminRouteConstants.AdminPersonnels);
    }
}
