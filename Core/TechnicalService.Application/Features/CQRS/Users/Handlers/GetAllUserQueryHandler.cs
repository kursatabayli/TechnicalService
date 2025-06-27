using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.Users.Queries;
using TechnicalService.Application.Features.CQRS.Users.Results;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Users.Handlers
{
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, Result<List<UserResult>>>
    {
        private readonly IRepository<User, Guid> _repository;
        private readonly ILogger<GetAllUserQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;

        public GetAllUserQueryHandler(IRepository<User, Guid> repository, ILogger<GetAllUserQueryHandler> logger, IMapper mapper, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _returnMessages = returnMessages;
        }

        public async Task<Result<List<UserResult>>> Handle(GetAllUserQuery request, CancellationToken ct)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_User_Plural];
            try
            {
                var users = await _repository.GetAllAsync();
                var mappedUsers = _mapper.Map<List<UserResult>>(users);
                return Result<List<UserResult>>.Success(mappedUsers, _returnMessages[ReturnMessages.Message_Success_Listed, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_List, entityName]);
                return Result<List<UserResult>>.Failure(_returnMessages[ReturnMessages.Error_Operation_List, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
