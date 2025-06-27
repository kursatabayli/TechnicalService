using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.Personnels.Queries;
using TechnicalService.Application.Features.CQRS.Personnels.Results;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Personnels.Handlers
{
    public class GetAllPersonnelsQueryHandler : IRequestHandler<GetAllPersonnelsQuery, Result<List<PersonnelResult>>>
    {
        private readonly IPersonnelRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllPersonnelsQueryHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public GetAllPersonnelsQueryHandler(IPersonnelRepository repository, IMapper mapper, ILogger<GetAllPersonnelsQueryHandler> logger, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _returnMessages = returnMessages;
        }

        public async Task<Result<List<PersonnelResult>>> Handle(GetAllPersonnelsQuery request, CancellationToken cancellationToken)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_Personnel_Plural];
            try
            {
                var personnels = await _repository.GetAllPersonnelsAsync();
                var mappedPersonnels = _mapper.Map<List<PersonnelResult>>(personnels);

                return Result<List<PersonnelResult>>.Success(mappedPersonnels, _returnMessages[ReturnMessages.Message_Success_Listed, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_List, entityName].ToString());

                return Result<List<PersonnelResult>>.Failure(_returnMessages[ReturnMessages.Error_Operation_List, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
