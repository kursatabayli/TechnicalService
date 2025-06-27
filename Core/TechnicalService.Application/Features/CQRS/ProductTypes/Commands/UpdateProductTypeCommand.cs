using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.ProductTypes.Commands
{
    public class UpdateProductTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }
}
