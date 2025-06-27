//using System.Security.Claims;

//namespace TechnicalService.WebAPI.StaticClasses
//{
//    public static class CurrentUser
//    {
//        private static IHttpContextAccessor _httpContextAccessor;

//        public static void Configure(IHttpContextAccessor httpContextAccessor)
//        {
//            _httpContextAccessor = httpContextAccessor;
//        }

//        public static Guid CurrentUserId
//        {
//            get
//            {
//                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//                if (string.IsNullOrEmpty(userIdClaim))
//                {
//                    throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı");
//                }
//                return Guid.Parse(userIdClaim);
//            }
//        }
//    }
//}
