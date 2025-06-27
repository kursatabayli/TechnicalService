using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.Products.Queries;
using TechnicalService.Application.Features.CQRS.Products.Results;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Products.Handlers
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, Result<List<ProductResult>>>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllProductQueryHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public GetAllProductQueryHandler(IProductRepository repository, IMapper mapper, ILogger<GetAllProductQueryHandler> logger, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _returnMessages = returnMessages;
        }

        public async Task<Result<List<ProductResult>>> Handle(GetAllProductQuery request, CancellationToken ct)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_Product_Plural];

            try
            {
                var products = await _repository.GetAllProductsAsync();
                var mappedProducts = _mapper.Map<List<ProductResult>>(products);

                return Result<List<ProductResult>>.Success(mappedProducts, _returnMessages[ReturnMessages.Message_Success_Listed, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_List, entityName]);

                return Result<List<ProductResult>>.Failure(_returnMessages[ReturnMessages.Error_Operation_List, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
