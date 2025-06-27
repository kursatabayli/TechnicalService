using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.UserProducts.Queries;
using TechnicalService.Application.Features.CQRS.UserProducts.Results;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.UserProducts.Handlers
{
    public class GetAllUserProductQueryHandler : IRequestHandler<GetAllUserProductQuery, Result<List<UserProductResult>>>
    {
        private readonly IUserProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllUserProductQueryHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public GetAllUserProductQueryHandler(IUserProductRepository repository, IMapper mapper, ILogger<GetAllUserProductQueryHandler> logger, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _returnMessages = returnMessages;
        }

        public async Task<Result<List<UserProductResult>>> Handle(GetAllUserProductQuery request, CancellationToken ct)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_UserProduct_Plural];
            try
            {
                var userProducts = await _repository.GetAllUserProductsAsync();
                var mappedUserProducts = _mapper.Map<List<UserProductResult>>(userProducts);
                return Result<List<UserProductResult>>.Success(mappedUserProducts, _returnMessages[ReturnMessages.Message_Success_Listed, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_List, entityName]);
                return Result<List<UserProductResult>>.Failure(_returnMessages[ReturnMessages.Error_Operation_List, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
