using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.UserProducts.Commands
{
    public class DeleteUserProductCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public DeleteUserProductCommand(int id)
        {
            Id = id;
        }
    }
}
