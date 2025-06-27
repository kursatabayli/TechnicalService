using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalService.Application.Features.CQRS.ServiceRecords.Results;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ServiceRecords.Queries
{
    public class GetServiceRecordsByServiceIdQuery : IRequest<Result<List<ServiceRecordListResult>>>
    {
        public Guid PersonnelId { get; set; }
        public GetServiceRecordsByServiceIdQuery(Guid personnelId)
        {
            PersonnelId = personnelId;
        }
    }
}
