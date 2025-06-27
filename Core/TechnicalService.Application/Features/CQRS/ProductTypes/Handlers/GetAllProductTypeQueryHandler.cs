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
    public class GetAllProductTypeQueryHandler : IRequestHandler<GetAllProductTypeQuery, Result<List<ProductTypeResult>>>
    {
        private readonly IRepository<ProductType, int> _repository;
        private readonly ILogger<GetAllProductTypeQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public GetAllProductTypeQueryHandler(IRepository<ProductType, int> repository, ILogger<GetAllProductTypeQueryHandler> logger, IMapper mapper, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _returnMessages = returnMessages;
        }

        public async Task<Result<List<ProductTypeResult>>> Handle(GetAllProductTypeQuery request, CancellationToken ct)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_ProductType_Plural];

            try
            {
                var productTypes = await _repository.GetAllAsync();
                var mappedProductTypes = _mapper.Map<List<ProductTypeResult>>(productTypes);

                return Result<List<ProductTypeResult>>.Success(mappedProductTypes, _returnMessages[ReturnMessages.Message_Success_Listed, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_List, entityName]);
                return Result<List<ProductTypeResult>>.Failure(_returnMessages[ReturnMessages.Error_Operation_List, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
