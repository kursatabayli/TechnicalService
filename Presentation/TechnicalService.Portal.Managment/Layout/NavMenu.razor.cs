using Microsoft.AspNetCore.Components;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.Portal.Managment.Layout
{
    public partial class NavMenu : ComponentBase, IDisposable
    {
        [Inject] IUIStateService UIStateService { get; set; }
        private UIStateOption UIStateOption { get; set; }

        protected override void OnInitialized()
        {
            UIStateService.OnChange += HandleStateChange;
            UIStateService.LoadUIState();
            UIStateOption = UIStateService.UIState;
            base.OnInitialized();
        }

        private async Task HandleDrawerOpenChanged(bool newValue)
        {
            var newState = UIStateOption with { drawerOpen = newValue };
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
