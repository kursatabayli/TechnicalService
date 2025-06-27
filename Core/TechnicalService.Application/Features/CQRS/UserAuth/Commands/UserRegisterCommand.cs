using MediatR;
using TechnicalService.Domain.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.UserAuth.Commands
{
    public class UserRegisterCommand : IRequest<Result>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Address { get; set; }
        public Gender? Gender { get; set; }
    }
}
