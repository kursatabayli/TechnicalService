using MudBlazor;
using TechnicalService.RazorHelpers.Shared;
using TechnicalService.Shared.Services.Contracts;

namespace TechnicalService.Shared.Services.Implementations
{
    public class DeleteDialogService : IDeleteDialogService
    {
        private readonly IDialogService _dialogService;

        public DeleteDialogService(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public async Task<bool> ShowDeleteDialogAsync(string title, string content, string submitButtonText)
        {
            var parameters = new DialogParameters<DeleteDialog>
                {
                    { x => x.Content, content },
                    { x => x.Title, title },
                    { x => x.SubmitButtonText, submitButtonText }
                };
            var dialog = await _dialogService.ShowAsync<DeleteDialog>(null, parameters);
            var result = await dialog.Result;
            return result != null && !result.Canceled && result.Data is true;
        }
    }
}
