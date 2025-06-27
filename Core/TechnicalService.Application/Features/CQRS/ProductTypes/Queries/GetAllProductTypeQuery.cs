using TechnicalService.Application.Features.CQRS.ProductTypes.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ProductTypes.Queries
{
    public class GetAllProductTypeQuery : IRequest<Result<List<ProductTypeResult>>>
    {
    }
}
