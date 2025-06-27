using TechnicalService.Application.Features.CQRS.UserProducts.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.UserProducts.Queries
{
    public class GetUserProductByIdQuery : IRequest<Result<UserProductResult>>
    {
        public int Id { get; set; }

        public GetUserProductByIdQuery(int id)
        {
            Id = id;
        }
    }
}
