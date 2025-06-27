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
    public class GetPersonnelsByServiceQueryHandler : IRequestHandler<GetPersonnelsByServiceQuery, Result<List<PersonnelMinimalResult>>>
    {
        private readonly IPersonnelRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPersonnelsByServiceQueryHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;

        public GetPersonnelsByServiceQueryHandler(IPersonnelRepository repository, IMapper mapper, ILogger<GetPersonnelsByServiceQueryHandler> logger, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _returnMessages = returnMessages;
        }

        public async Task<Result<List<PersonnelMinimalResult>>> Handle(GetPersonnelsByServiceQuery request, CancellationToken cancellationToken)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_Personnel_Plural];
            try
            {
                var personnel = await _repository.GetByIdAsync(request.PersonnelId);
                var personnels = await _repository.GetPersonnelsByServiceAsync(personnel.TechnicalServiceId);
                var mappedPersonnels = _mapper.Map<List<PersonnelMinimalResult>>(personnels);

                return Result<List<PersonnelMinimalResult>>.Success(mappedPersonnels, _returnMessages[ReturnMessages.Message_Success_Listed, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_List, entityName].ToString());

                return Result<List<PersonnelMinimalResult>>.Failure(_returnMessages[ReturnMessages.Error_Operation_List, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
