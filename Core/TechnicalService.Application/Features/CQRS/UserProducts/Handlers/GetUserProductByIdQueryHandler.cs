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
    public class GetUserProductByIdQueryHandler : IRequestHandler<GetUserProductByIdQuery, Result<UserProductResult>>
    {
        private readonly IUserProductRepository _repository;
        private readonly ILogger<GetUserProductByIdQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;

        public GetUserProductByIdQueryHandler(IUserProductRepository repository, ILogger<GetUserProductByIdQueryHandler> logger, IMapper mapper, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _returnMessages = returnMessages;
        }

        public async Task<Result<UserProductResult>> Handle(GetUserProductByIdQuery request, CancellationToken ct)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_UserProduct];
            var userProduct = await _repository.GetUserProductByIdAsync(request.Id);

            if (userProduct == null)
                return Result<UserProductResult>.Failure(_returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            try
            {
                var mappedUserProduct = _mapper.Map<UserProductResult>(userProduct);
                return Result<UserProductResult>.Success(mappedUserProduct, _returnMessages[ReturnMessages.Message_Success_Retrieved, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Retrieve, entityName], request.Id);
                return Result<UserProductResult>.Failure(_returnMessages[ReturnMessages.Error_Operation_Retrieve, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
