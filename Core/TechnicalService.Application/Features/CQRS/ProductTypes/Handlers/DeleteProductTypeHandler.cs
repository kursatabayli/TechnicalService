using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.ProductTypes.Commands;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ProductTypes.Handlers
{
    public class DeleteProductTypeHandler : IRequestHandler<DeleteProductTypeCommand, Result<int>>
    {
        private readonly IRepository<ProductType, int> _repository;
        private readonly ILogger<DeleteProductTypeCommand> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public DeleteProductTypeHandler(IRepository<ProductType, int> repository, ILogger<DeleteProductTypeCommand> logger, IUnitOfWork unitOfWork, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _returnMessages = returnMessages;
        }

        public async Task<Result<int>> Handle(DeleteProductTypeCommand request, CancellationToken ct)
        {
            var productType = await _repository.GetByIdAsync(request.Id);
            var entityName = _returnMessages[ReturnMessages.EntityType_ProductType];

            if (productType == null)
                return Result<int>.Failure(request.Id, _returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            try
            {
                await _repository.DeleteAsync(productType);
                await _unitOfWork.SaveChangesAsync();
                return Result<int>.Success(request.Id, _returnMessages[ReturnMessages.Message_Success_Deleted, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Delete, entityName], request.Id);
                return Result<int>.Failure(request.Id, _returnMessages[ReturnMessages.Error_Operation_Delete, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
