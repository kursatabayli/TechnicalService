using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace TechnicalService.Shared.Extensions
{
    public static class NavigationOnSnackbar
    {
        public static void NavigateOnClick(this ISnackbar snackbar, NavigationManager navigationManager, string message, string url, string title = "Görüntüle", Func<Task> onClickAction = null)
        {
            snackbar.Add(message, Severity.Warning, config =>
            {
                config.Action = title;
                config.ActionColor = Color.Warning;
                config.ActionVariant = Variant.Text;
                config.CloseAfterNavigation = true;
                config.OnClick = async snackbar =>
                {
                    if (onClickAction != null)
                    {
                        await onClickAction.Invoke();
                    }
                    navigationManager.NavigateTo(url);
                };
            });
        }
    }
}
