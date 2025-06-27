using Microsoft.AspNetCore.Components;
using MudBlazor;
using TechnicalService.DTOs.DTOs.PersonnelDTOs;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.Portal.OperationalStaff.Pages.Profile
{
    public partial class Profile : ComponentBase
    {
        private PersonnelDto PersonnelDto;
        private bool isLoading = true;
        [Inject] private IDataService<Result<PersonnelDto>> DataService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var result = await DataService.GetAsync(Endpoints.CurrentPersonnel, ClientTypes.PersonnelAuthClient);
            if (result.IsSuccess)
            {
                PersonnelDto = result.Data;
            }
            else
            {
                Snackbar.Add(result.StatusMessage, Severity.Error);
                NavigationManager.NavigateTo("/", true);
            }
        }

        private static string StatusIcon(PersonnelStatusDto status) => status switch
        {
            PersonnelStatusDto.Active => Icons.Material.Filled.CheckCircle,
            PersonnelStatusDto.Suspended => Icons.Material.Filled.PauseCircle,
            PersonnelStatusDto.OnLeave => Icons.Material.Filled.AccessTime,
            PersonnelStatusDto.Terminated => Icons.Material.Filled.Cancel,
            _ => Icons.Material.Filled.HelpOutline
        };

        private static Color StatusColor(PersonnelStatusDto status) => status switch
        {
            PersonnelStatusDto.Active => Color.Success,
            PersonnelStatusDto.Suspended => Color.Inherit,
            PersonnelStatusDto.OnLeave => Color.Warning,
            PersonnelStatusDto.Terminated => Color.Error,
            _ => Color.Default
        };


        private void NavigatePage(string page) => NavigationManager.NavigateTo(page);
    }
}
