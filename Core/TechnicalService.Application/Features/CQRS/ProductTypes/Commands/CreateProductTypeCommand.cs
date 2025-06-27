using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ProductTypes.Commands
{
    public class CreateProductTypeCommand : IRequest<Result<int>>
    {
        public string Type { get; set; }
    }
}
