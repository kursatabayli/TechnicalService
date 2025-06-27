using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;
using TechnicalService.DTOs.DTOs.UserProductDTOs;
using TechnicalService.RazorHelpers.Shared;
using TechnicalService.DTOs.Results;
using TechnicalService.Shared.Constants;
using TechnicalService.Shared.Services.Contracts;
using TechnicalService.Validations.Common.Validations.UserProductValidators;

namespace TechnicalService.UserUI.Pages.UserProducts
{
    public partial class AddProduct : ComponentBase
    {
        private MudForm form;
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
        private AddUserProductDto AddUserProduct { get; set; } = new();
        private AddUserProductsValidator Validator { get; set; } = new();
        [Inject] private IDataService<AddUserProductDto> dataService { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        private bool IsSubmitting { get; set; } = false;
        protected override void OnInitialized()
        {
            DialogStyles();
        }
        private async Task Submit()
        {
            if (!await ValidateFormAsync())
                return;

            IsSubmitting = true;
            try
            {
                var result = await dataService.CreateAsync(Endpoints.AddUserProduct, AddUserProduct, ClientTypes.UserAuthClient);
                var response = await result.Content.ReadFromJsonAsync<Result>();
                Cancel();
                if (response.IsSuccess)
                    await OpenSimpleTextDialogAsync(
                                    "Ürün Kaydı",
                                    response.StatusMessage,
                                    Icons.Material.Filled.CheckCircle,
                                    Color.Success
                                );
                else
                    await OpenSimpleTextDialogAsync(
                                    "Kayıt Hatası",
                                    response.StatusMessage,
                                    Icons.Material.Filled.Cancel,
                                    Color.Error
                                );
            }
            catch (Exception ex)
            {
                Cancel();
                await OpenSimpleTextDialogAsync(
                            "Hata",
                            "Teknik bir sorun oluştu: " + ex.Message,
                            Icons.Material.Filled.Warning,
                            Color.Warning
                        );
            }
            finally
            {
                IsSubmitting = false;
            }

        }
        private async Task<bool> ValidateFormAsync()
        {
            await form.Validate();
            return form.IsValid;
        }
        private void Cancel() => MudDialog.Cancel();



        private Task DialogStyles()
        {
            var options = MudDialog.Options with
            {
                MaxWidth = MaxWidth.Small,
                FullWidth = true,
            };
            return MudDialog.SetOptionsAsync(options);
        }

        private async Task OpenSimpleTextDialogAsync(string title, string content, string contentIcon, Color generalColor)
        {

            var parameters = new DialogParameters<SimpleResponseDialog>
            {
                { x => x.Content,  content},
                { x => x.Title, title },
                { x => x.ContentIcon, contentIcon },
                { x => x.GeneralColor, generalColor },
            };

            var dialog = await DialogService.ShowAsync<SimpleResponseDialog>(null, parameters);
        }
    }
}
