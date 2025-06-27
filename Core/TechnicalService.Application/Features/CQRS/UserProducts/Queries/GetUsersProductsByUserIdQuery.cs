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
    public class GetUsersProductsByUserIdQuery : IRequest<Result<List<UserProductResult>>>
    {
        public Guid UserId { get; set; }

        public GetUsersProductsByUserIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
