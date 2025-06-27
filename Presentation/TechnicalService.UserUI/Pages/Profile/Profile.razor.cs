using Microsoft.AspNetCore.Components;
using MudBlazor;
using TechnicalService.DTOs.DTOs.UserDTOs;
using TechnicalService.DTOs.Results;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.UserUI.Pages.Profile
{
    public partial class Profile : ComponentBase
    {
        private UserDto dto;
        [Inject] private IDataService<Result<UserDto>> DataService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var result = await DataService.GetAsync(Endpoints.GetUser, ClientTypes.UserAuthClient);
            if (result.IsSuccess)
                dto = result.Data;
            else
                Snackbar.Add(result.StatusMessage, Severity.Error);
        }
    }
}
