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
    internal class GetPersonnelByIdQueryHandler : IRequestHandler<GetPersonnelByIdQuery, Result<PersonnelResult>>
    {
        private readonly IPersonnelRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPersonnelByIdQueryHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public GetPersonnelByIdQueryHandler(IPersonnelRepository repository, IMapper mapper, ILogger<GetPersonnelByIdQueryHandler> logger, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _returnMessages = returnMessages;
        }

        public async Task<Result<PersonnelResult>> Handle(GetPersonnelByIdQuery request, CancellationToken cancellationToken)
        {
            var personnel = await _repository.GetPersonnelByIdAsync(request.Id);
            var entityName = _returnMessages[ReturnMessages.EntityType_Personnel];

            if (personnel == null)
                return Result<PersonnelResult>.Failure(_returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);
            try
            {
                var mappedPersonnel = _mapper.Map<PersonnelResult>(personnel);

                return Result<PersonnelResult>.Success(mappedPersonnel, _returnMessages[ReturnMessages.Message_Success_Retrieved, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Retrieve, entityName].ToString());

                return Result<PersonnelResult>.Failure(_returnMessages[ReturnMessages.Error_Operation_Retrieve, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
