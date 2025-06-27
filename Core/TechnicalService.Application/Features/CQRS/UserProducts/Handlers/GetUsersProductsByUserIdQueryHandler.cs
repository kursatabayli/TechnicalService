using TechnicalService.Application.Features.CQRS.UserProducts.Results;
using TechnicalService.Application.Features.CQRS.UserProducts.Queries;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.DTOs.Results;
using System.Net;
using TechnicalService.DTOs.Enums;
using TechnicalService.Application.Extensions;
using Microsoft.Extensions.Localization;

namespace TechnicalService.Application.Features.CQRS.UserProducts.Handlers
{
    public class GetUsersProductsByUserIdQueryHandler : IRequestHandler<GetUsersProductsByUserIdQuery, Result<List<UserProductResult>>>
    {
        private readonly IUserProductRepository _repository;
        private readonly ILogger<GetUsersProductsByUserIdQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;

        public GetUsersProductsByUserIdQueryHandler(IUserProductRepository repository, ILogger<GetUsersProductsByUserIdQueryHandler> logger, IMapper mapper, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _returnMessages = returnMessages;
        }

        public async Task<Result<List<UserProductResult>>> Handle(GetUsersProductsByUserIdQuery request, CancellationToken ct)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_UserProduct_Plural];
            try
            {
                // Repository metodu zaten spesifik (_repository.GetUsersProductsByUserIdAsync)
                var userProducts = await _repository.GetUsersProductsByUserIdAsync(request.UserId);
                var mappedUserProducts = _mapper.Map<List<UserProductResult>>(userProducts);
                // Kullanıcıya ait ürün bulunamaması bir hata değil, boş liste dönülebilir.
                return Result<List<UserProductResult>>.Success(mappedUserProducts, _returnMessages[ReturnMessages.Message_Success_Listed, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_List, entityName], request.UserId);
                return Result<List<UserProductResult>>.Failure(_returnMessages[ReturnMessages.Error_Operation_List, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
