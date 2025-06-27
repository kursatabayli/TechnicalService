using TechnicalService.Application.Features.CQRS.Brands.Commands;
using TechnicalService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.DTOs.Results;
using System.Net;
using TechnicalService.DTOs.Enums;
using Microsoft.Extensions.Localization;
using TechnicalService.Application.Extensions;

namespace TechnicalService.Application.Features.CQRS.Brands.Handlers
{
    public class DeleteBrandHandler : IRequestHandler<DeleteBrandCommand, Result<int>>
    {
        private readonly IRepository<Brand, int> _repository;
        private readonly ILogger<DeleteBrandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public DeleteBrandHandler(IRepository<Brand, int> repository, ILogger<DeleteBrandHandler> logger, IUnitOfWork unitOfWork, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _returnMessages = returnMessages;
        }

        public async Task<Result<int>> Handle(DeleteBrandCommand request, CancellationToken ct)
        {
            var brand = await _repository.GetByIdAsync(request.Id);

            var entityName = _returnMessages[ReturnMessages.EntityType_Brand];

            if (brand == null)
                return Result<int>.Failure(request.Id, _returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            try
            {
                await _repository.DeleteAsync(brand);
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
