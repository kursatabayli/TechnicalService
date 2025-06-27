using Microsoft.JSInterop;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.Shared.Services.Implementations
{
    public class UIStateService : IUIStateService
    {
        private readonly IJSRuntime _jsRuntime;

        public UIStateService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            LoadUIState(); // Uygulama başlarken state'i yükle
        }

        public event Action? OnChange;
        public UIStateOption UIState { get; private set; } = new(false, false);

        public async void LoadUIState()
        {
            // localStorage'dan verileri oku
            var isDarkMode = await GetLocalStorageItem<bool>("Theme", true);
            var drawerOpen = await GetLocalStorageItem<bool>("Sidebar", true);

            UIState = new UIStateOption(isDarkMode, drawerOpen);
            OnChange?.Invoke();
        }

        public async void SetUIState(UIStateOption uiState)
        {
            // localStorage'a kaydet
            await SetLocalStorageItem("Theme", uiState.isDarkMode);
            await SetLocalStorageItem("Sidebar", uiState.drawerOpen);

            UIState = uiState;
            OnChange?.Invoke();
        }

        private async Task<T> GetLocalStorageItem<T>(string key, T defaultValue)
        {
            try
            {
                var value = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
                return value != null ? JsonSerializer.Deserialize<T>(value) : defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        private async Task SetLocalStorageItem<T>(string key, T value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
        }
    }
}
