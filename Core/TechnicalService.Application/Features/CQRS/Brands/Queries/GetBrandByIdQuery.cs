using TechnicalService.Application.Features.CQRS.Brands.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Brands.Queries
{
    public class GetBrandByIdQuery : IRequest<Result<BrandResult>>
    {
        public int Id { get; set; }

        public GetBrandByIdQuery(int ıd)
        {
            Id = ıd;
        }
    }
}
