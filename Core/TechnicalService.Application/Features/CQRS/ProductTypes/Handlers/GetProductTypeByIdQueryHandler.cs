using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.ProductTypes.Queries;
using TechnicalService.Application.Features.CQRS.ProductTypes.Results;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ProductTypes.Handlers
{
    public class GetProductTypeByIdQueryHandler : IRequestHandler<GetProductTypeByIdQuery, Result<ProductTypeResult>>
    {
        private readonly IRepository<ProductType, int> _repository;
        private readonly ILogger<GetProductTypeByIdQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public GetProductTypeByIdQueryHandler(IRepository<ProductType, int> repository, ILogger<GetProductTypeByIdQueryHandler> logger, IMapper mapper, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _returnMessages = returnMessages;
        }

        public async Task<Result<ProductTypeResult>> Handle(GetProductTypeByIdQuery request, CancellationToken ct)
        {
            var productType = await _repository.GetByIdAsync(request.Id);
            var entityName = _returnMessages[ReturnMessages.EntityType_ProductType];

            if (productType == null)
                return Result<ProductTypeResult>.Failure(_returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            try
            {
                var mappedProductType = _mapper.Map<ProductTypeResult>(productType);
                return Result<ProductTypeResult>.Success(mappedProductType, _returnMessages[ReturnMessages.Message_Success_Retrieved, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Retrieve, entityName], request.Id);
                return Result<ProductTypeResult>.Failure(_returnMessages[ReturnMessages.Error_Operation_Retrieve, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
