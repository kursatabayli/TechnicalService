using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.UserProducts.Commands
{
    public class UpdateUserProductCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int SerialNumberId { get; set; }
        public Guid UserId { get; set; }
    }
}
