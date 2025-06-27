using TechnicalService.Domain.Enums;

namespace TechnicalService.Application.Features.CQRS.Users.Results
{
    public class UserResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Address { get; set; }
        public Gender? Gender { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }

    }
}
