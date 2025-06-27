using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace TechnicalService.RazorHelpers.Shared
{
    public partial class DeleteDialog : ComponentBase
    {
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public string Content { get; set; }
        [Parameter] public string SubmitButtonText { get; set; }

        private void Submit() => MudDialog.Close(DialogResult.Ok(true));
        private void Cancel() => MudDialog.Cancel();
    }
}
