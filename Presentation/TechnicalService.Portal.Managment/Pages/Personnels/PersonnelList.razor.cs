using Microsoft.AspNetCore.Components;
using MudBlazor;
using TechnicalService.DTOs.DTOs.PersonnelDTOs;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.Portal.Managment.Pages.Personnels
{
    public partial class PersonnelList : ComponentBase
    {
        private List<PersonnelDto> dto;
        private bool LoadingData { get; set; } = true;
        private string SearchText { get; set; } = string.Empty;
        [Inject] private IDataService<Result<List<PersonnelDto>>> GetPersonnelDataService { get; set; }
        [Inject] private ISearchService SearchService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var personnels = await GetPersonnelDataService.GetAsync(Endpoints.GetPersonnelsByService, ClientTypes.PersonnelAuthClient);
            if (personnels.IsSuccess)
            {
                dto = personnels.Data;
            }
            else
            {
                Snackbar.Add(personnels.StatusMessage, Severity.Error);
                dto = [];
            }
            LoadingData = false;
        }

        private Color GetStatusColor(PersonnelStatusDto status)
            => status switch
            {
                PersonnelStatusDto.Active => Color.Success,
                PersonnelStatusDto.OnLeave => Color.Info,
                PersonnelStatusDto.Suspended => Color.Warning,
                PersonnelStatusDto.Terminated => Color.Error,
                _ => throw new NotImplementedException(),
            };

        private IEnumerable<PersonnelDto> FilteredRequests =>
            SearchService.Search(SearchText, dto, b => [$"{b.Name} {b.Surname}", b.Email]) ?? [];
        private void NavigateToPage(string page) => NavigationManager.NavigateTo(page);

    }
}
