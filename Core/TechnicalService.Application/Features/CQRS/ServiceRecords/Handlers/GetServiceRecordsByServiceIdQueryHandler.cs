using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.ServiceRecords.Queries;
using TechnicalService.Application.Features.CQRS.ServiceRecords.Results;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecords.Handlers
{
    internal class GetServiceRecordsByServiceIdQueryHandler : IRequestHandler<GetServiceRecordsByServiceIdQuery, Result<List<ServiceRecordListResult>>>
    {
        private readonly IServiceRecordRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetServiceRecordsByServiceIdQueryHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        private readonly IRepository<Personnel, Guid> _personnelRepository;

        public GetServiceRecordsByServiceIdQueryHandler(IServiceRecordRepository repository, IMapper mapper, ILogger<GetServiceRecordsByServiceIdQueryHandler> logger, IStringLocalizer<ReturnMessages> returnMessages, IRepository<Personnel, Guid> personnelRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _returnMessages = returnMessages;
            _personnelRepository = personnelRepository;
        }

        public async Task<Result<List<ServiceRecordListResult>>> Handle(GetServiceRecordsByServiceIdQuery request, CancellationToken cancellationToken)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_ServiceRecord_Plural];


            try
            {
                var personnel = await _personnelRepository.GetByIdAsync(request.PersonnelId);
                if (personnel == null)
                {
                    _logger.LogError(_returnMessages[ReturnMessages.Error_Operation_List, entityName]);
                    return Result<List<ServiceRecordListResult>>.Failure(_returnMessages[ReturnMessages.Error_Operation_List, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.NotFound, HttpStatusCode.NotFound);
                }
                var serviceId = personnel.TechnicalServiceId;
                var serviceRecords = await _repository.GetServiceRecordsByServiceIdAsync(serviceId);

                var mappedServiceRecords = _mapper.Map<List<ServiceRecordListResult>>(serviceRecords);
                return Result<List<ServiceRecordListResult>>.Success(mappedServiceRecords, _returnMessages[ReturnMessages.Message_Success_Listed, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_List, entityName]);
                return Result<List<ServiceRecordListResult>>.Failure(_returnMessages[ReturnMessages.Error_Operation_List, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
