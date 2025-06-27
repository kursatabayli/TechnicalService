namespace TechnicalService.Shared.Services.Contracts
{
    public interface IDeleteDialogService
    {
        Task<bool> ShowDeleteDialogAsync(string title, string content, string submitButtonText);
    }
}
