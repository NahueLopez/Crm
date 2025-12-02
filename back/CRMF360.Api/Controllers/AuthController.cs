using CRMF360.Application.Auth;
using Microsoft.AspNetCore.Mvc;

namespace CRMF360.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public record LoginRequest(string UsernameOrEmail, string Password);

        public record LoginResponse(string Token, string FullName, string Email);

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request.UsernameOrEmail, request.Password);

            if (result is null)
                return Unauthorized(new { message = "Usuario o contraseña incorrectos" });

            return Ok(new LoginResponse(result.Token, result.FullName, result.Email));
        }
    }
}
