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
    public class CreateProductTypeHandler : IRequestHandler<CreateProductTypeCommand, Result<int>>
    {
        private readonly IRepository<ProductType, int> _repository;
        private readonly ILogger<CreateProductTypeHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;

        public CreateProductTypeHandler(IRepository<ProductType, int> repository, ILogger<CreateProductTypeHandler> logger, IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _returnMessages = returnMessages;
        }

        public async Task<Result<int>> Handle(CreateProductTypeCommand request, CancellationToken ct)
        {
            var productType = _mapper.Map<ProductType>(request);
            var entityName = _returnMessages[ReturnMessages.EntityType_ProductType];

            var existingProductType = await _repository.GetFirstOrDefaultAsync(pt => pt.Type.ToLower() == productType.Type.ToLower());

            if (existingProductType != null)
                return Result<int>.Failure(existingProductType.Id, _returnMessages[ReturnMessages.Error_Entity_AlreadyExists_WithName, request.Type, entityName], StatusCode.Conflict, HttpStatusCode.Conflict);

            try
            {
                await _repository.CreateAsync(productType);
                await _unitOfWork.SaveChangesAsync();
                return Result<int>.Success(productType.Id, _returnMessages[ReturnMessages.Message_Success_Created_WithName, productType.Type, entityName], HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Create, entityName], request.Type);
                return Result<int>.Failure(_returnMessages[ReturnMessages.Error_Operation_Create, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
