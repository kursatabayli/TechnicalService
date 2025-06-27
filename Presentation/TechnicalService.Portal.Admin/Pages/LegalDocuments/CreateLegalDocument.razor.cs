using Blazored.TextEditor;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.LegalDocumentDTOs;
using TechnicalService.DTOs.Results;
using TechnicalService.Portal.Admin.Constants;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.Validations.Common.Validations.LegalDocumentValidators;

namespace TechnicalService.Portal.Admin.Pages.LegalDocuments
{
    public partial class CreateLegalDocument : ComponentBase, IDisposable
    {
        [Inject] private IDataService<CreateLegalDocumentDto> CreateLegalDocumentDataService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        private CreateLegalDocumentDto CreateLegalDocumentDto = new();
        private CreateLegalDocumentValidator Validator = new();
        private MudForm form;
        private BlazoredTextEditor editorInstance;

        private string previewContent;
        private bool isSaving = false;

        private System.Timers.Timer previewTimer;

        protected override void OnInitialized()
        {
            previewTimer = new System.Timers.Timer(500);
            previewTimer.Elapsed += async (sender, e) => await OnTimerElapsed();
            previewTimer.AutoReset = true;
        }

        private async Task OnTimerElapsed()
        {
            await RefreshPreviewAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OnDocumentTypeChanged()
        {
            previewContent = string.Empty;
            if (editorInstance != null)
            {
                await editorInstance.LoadHTMLContent(string.Empty);
            }

            if (CreateLegalDocumentDto.DocumentType != null)
            {
                previewTimer.Start();
            }
            else
            {
                previewTimer.Stop();
            }
            StateHasChanged();
        }

        private async Task RefreshPreviewAsync()
        {
            if (editorInstance != null)
            {
                previewContent = await editorInstance.GetHTML();
            }
        }
        private async Task<bool> ValidateFormAsync()
        {
            await form.Validate();
            return form.IsValid;
        }
        private async Task Submit()
        {
            isSaving = true;
            previewTimer.Stop();

            if (editorInstance != null)
            {
                CreateLegalDocumentDto.Content = await editorInstance.GetHTML();
            }

            if (!await ValidateFormAsync())
            {
                isSaving = false;
                previewTimer.Start();
                return;
            }

            var response = await SendData();

            if (response.IsSuccess)
            {
                Snackbar.Add(response.StatusMessage, Severity.Success);
                NavigationManager.NavigateTo(AdminRouteConstants.AdminLegalDocuments);
            }
            else
            {
                Snackbar.Add(response.StatusMessage, Severity.Error);
                previewTimer.Start();
            }

            isSaving = false;
        }

        private async Task<Result<int>> SendData()
        {
            var result = await CreateLegalDocumentDataService.CreateAsync(Endpoints.CreateLegalDocument, CreateLegalDocumentDto, ClientTypes.PersonnelAuthClient);
            return await result.Content.ReadFromJsonAsync<Result<int>>();
        }
        private void NavigateBack() => NavigationManager.NavigateTo(AdminRouteConstants.AdminLegalDocuments);

        public void Dispose()
        {
            previewTimer?.Dispose();
        }
    }
}