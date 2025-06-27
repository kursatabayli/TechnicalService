using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalService.DTOs.DTOs.PersonnelDTOs;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.Portal.Managment.Pages.Personnels
{
    public partial class PersonnelDetail : ComponentBase
    {
        [Parameter] public Guid Id { get; set; }
        private PersonnelDto _personnel = new();
        private bool _isLoading = true;
        [Inject] private IDataService<Result<PersonnelDto>> GetPersonnelDataService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var personnels = await GetPersonnelDataService.GetAsync(Endpoints.GetPersonnelById + Id, ClientTypes.PersonnelAuthClient);
            if (personnels.IsSuccess)
                _personnel = personnels.Data;
            else
                Snackbar.Add(personnels.StatusMessage, Severity.Error);
            _isLoading = false;
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
        private string GetGenderIcon(GenderDto gender) 
            => gender switch
            {
                GenderDto.Male => Icons.Material.Filled.Male,
                GenderDto.Female => Icons.Material.Filled.Female,
                _ => Icons.Material.Filled.Wc
            };
    }
}
