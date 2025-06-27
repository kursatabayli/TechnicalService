using AutoMapper;
using TechnicalService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.DTOs.Results;
using System.Net;
using TechnicalService.Application.Features.CQRS.UserAuth.Commands;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.Application.Features.CQRS.UserAuth.Handlers
{
    public class UserRegisterHandler : IRequestHandler<UserRegisterCommand, Result>
    {
        private readonly IRepository<User, Result> _userRepository;
        private readonly IRepository<EmailVerificationToken, Guid> _tokenRepository;
        private readonly ILogger<UserRegisterHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHashService _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly IAuthService _authService;
        public UserRegisterHandler(IRepository<User, Result> userRepository, ILogger<UserRegisterHandler> logger, IUnitOfWork unitOfWork, IMapper mapper, IHashService passwordHasher, IEmailService emailService, IAuthService authService, IRepository<EmailVerificationToken, Guid> tokenRepository)
        {
            _userRepository = userRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _authService = authService;
            _tokenRepository = tokenRepository;
        }

        public async Task<Result> Handle(UserRegisterCommand request, CancellationToken ct)
        {
            var user = _mapper.Map<User>(request);

            var existingUser = await _userRepository.GetFirstOrDefaultAsync(x => x.Email == user.Email);

            if (existingUser != null)
                return Result.Failure("Bu e-posta adresi zaten kullanılmaktadır.", StatusCode.UserAlreadyExists, HttpStatusCode.NotAcceptable);

            try
            {
                var (hash, salt) = _passwordHasher.HashItem(request.Password);
                user.PasswordHash = hash;
                user.PasswordSalt = salt;
                user.Id = Guid.NewGuid();
                var verificationToken = _authService.GenerateEmailVerificationToken(user.Email);

                var writeTokenOnDb = new EmailVerificationToken
                {
                    Token = verificationToken,
                    UserId = user.Id,
                    Expiry = DateTime.UtcNow.AddHours(1),
                };


                await _userRepository.CreateAsync(user);
                await _tokenRepository.CreateAsync(writeTokenOnDb);
                await _unitOfWork.SaveChangesWithTransactionAsync();


                // E-posta doğrulama token'ı oluştur ve e-posta gönder
                await _emailService.SendVerificationEmailAsync(request.Email, verificationToken);
                return Result.Success("Kayıt İşlemi Başarılı!");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı oluşturma hatası. Kullanıcı Adı: {Name}", request.Name);
                return Result.Failure("Beklenmedik bir hata oluştu.", StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}
