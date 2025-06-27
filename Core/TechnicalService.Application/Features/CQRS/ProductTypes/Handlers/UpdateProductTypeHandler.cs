using AutoMapper;
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
    public class UpdateProductTypeHandler : IRequestHandler<UpdateProductTypeCommand, Result<int>>
    {
        private readonly IRepository<ProductType, int> _repository;
        private readonly ILogger<UpdateProductTypeHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public UpdateProductTypeHandler(IRepository<ProductType, int> repository, ILogger<UpdateProductTypeHandler> logger, IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _returnMessages = returnMessages;
        }

        public async Task<Result<int>> Handle(UpdateProductTypeCommand request, CancellationToken ct)
        {
            var productType = await _repository.GetByIdAsync(request.Id);
            var entityName = _returnMessages[ReturnMessages.EntityType_ProductType];

            if (productType == null)
                return Result<int>.Failure(_returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            var existingProductType = await _repository.GetFirstOrDefaultAsync(pt =>
                                                pt.Type.ToLower() == request.Type.ToLower() &&
                                                pt.Id != request.Id);

            if (existingProductType != null)
                return Result<int>.Failure(existingProductType.Id, _returnMessages[ReturnMessages.Error_Entity_AlreadyExists_WithName, request.Type, entityName], StatusCode.Conflict, HttpStatusCode.Conflict);

            try
            {
                _mapper.Map(request, productType);
                await _repository.UpdateAsync(productType);
                await _unitOfWork.SaveChangesAsync();
                return Result<int>.Success(productType.Id, _returnMessages[ReturnMessages.Message_Success_Updated_WithName, productType.Type, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Update, entityName], request.Id);
                return Result<int>.Failure(_returnMessages[ReturnMessages.Error_Operation_Update, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
