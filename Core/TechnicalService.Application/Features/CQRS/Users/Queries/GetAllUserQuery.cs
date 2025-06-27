using TechnicalService.Application.Features.CQRS.Users.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Users.Queries
{
    public class GetAllUserQuery : IRequest<Result<List<UserResult>>>
    {
    }
}
