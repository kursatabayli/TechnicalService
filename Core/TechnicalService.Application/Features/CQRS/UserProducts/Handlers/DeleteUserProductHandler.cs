using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.UserProducts.Commands;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.UserProducts.Handlers
{
    public class DeleteUserProductHandler : IRequestHandler<DeleteUserProductCommand, Result<int>>
    {
        private readonly IRepository<UserProduct, int> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteUserProductHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public DeleteUserProductHandler(IRepository<UserProduct, int> repository, IUnitOfWork unitOfWork, ILogger<DeleteUserProductHandler> logger, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _returnMessages = returnMessages;
        }

        public async Task<Result<int>> Handle(DeleteUserProductCommand request, CancellationToken ct)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_UserProduct];
            var userProduct = await _repository.GetByIdAsync(request.Id);

            if (userProduct == null)
                return Result<int>.Failure(request.Id, _returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            try
            {
                await _repository.DeleteAsync(userProduct);
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
