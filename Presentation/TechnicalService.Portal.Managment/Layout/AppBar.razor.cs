using Microsoft.AspNetCore.Components;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.Portal.Managment.Layout
{
    public partial class AppBar : ComponentBase, IDisposable
    {
        [Inject] private IUIStateService UIStateService { get; set; }
        private UIStateOption UIStateOption { get; set; }

        protected override void OnInitialized()
        {
            UIStateService.OnChange += HandleStateChange;
            UIStateService.LoadUIState();
            UIStateOption = UIStateService.UIState;
            base.OnInitialized();
        }

        private void DrawerToggle()
        {
            var newState = UIStateOption with { drawerOpen = !UIStateOption.drawerOpen };
            UIStateService.SetUIState(newState);
        }

        private void DarkModeToggle()
        {
            var newState = UIStateOption with { isDarkMode = !UIStateOption.isDarkMode };
            UIStateService.SetUIState(newState);
        }

        private void HandleStateChange()
        {
            UIStateOption = UIStateService.UIState;
            StateHasChanged();
        }

        public void Dispose() => UIStateService.OnChange -= HandleStateChange;
    }
}
