using MediatR;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Application.Features.CQRS.UserAuth.Commands;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Results;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.Application.Features.CQRS.UserAuth.Handlers
{
    public class UserRequestResetPasswordLinkHandler : IRequestHandler<UserRequestResetPasswordLinkCommand, Result>
    {
        private readonly IRepository<User, Guid> _repository;
        private readonly IEmailService _emailService;
        private readonly IAuthService _authService;

        public UserRequestResetPasswordLinkHandler(IRepository<User, Guid> repository, IEmailService emailService, IAuthService authService)
        {
            _repository = repository;
            _emailService = emailService;
            _authService = authService;
        }

        public async Task<Result> Handle(UserRequestResetPasswordLinkCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetFirstOrDefaultAsync(x => x.Email == request.Email);
            if (user == null)
                return Result.Failure("Kullanıcı Bulunamadı.", StatusCode.NotFound, HttpStatusCode.NotFound);

            try
            {
                var resetTOken = _authService.GeneratePasswordResetToken(user.Id.ToString());
                await _emailService.SendPasswordResetEmailAsync(request.Email, resetTOken);
                return Result.Success("Şifre sıfırlama bağlantısı e-posta adresinize gönderildi.");
            }
            catch (Exception ex)
            {
                return Result.Failure("Sıfırlama bağlantısı gönderilirken bir sorun oluştu lütfen daha sonra tekrar deneyiniz.", StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }

        }

    }
}
