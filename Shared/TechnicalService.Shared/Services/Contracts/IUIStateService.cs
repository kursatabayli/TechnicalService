using TechnicalService.Shared.Constants;

namespace TechnicalService.Shared.Services.Contracts
{
    public interface IUIStateService
    {
        event Action OnChange;
        UIStateOption UIState { get; }
        void SetUIState(UIStateOption uiState);
        void LoadUIState();
    }
}
