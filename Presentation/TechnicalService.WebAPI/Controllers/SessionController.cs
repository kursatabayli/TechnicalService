using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechnicalService.DTOs.DTOs.AuthDTOs;

namespace TechnicalService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {

        [HttpGet("Check")]
        public IActionResult CheckAuth()
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var userClaims = new UserClaims
            {
                Id = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                Email = User.FindFirstValue(ClaimTypes.Email),
                Name = User.FindFirstValue(ClaimTypes.Name),
                LastName = User.FindFirstValue(ClaimTypes.Surname),
                Role = User.FindFirstValue(ClaimTypes.Role),
            };

            return Ok(userClaims);
        }


    }
}
