// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using server.Dtos.UserDto;
using server.Interfaces;

namespace server.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto)
        {
            var result = await _authService.SignUpAsync(signUpDto);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInDto model)
        {
            var result = await _authService.SignInAsync(model);
            if (!result.IsSuccess)
                return Unauthorized(new { Status = "Error", result.Message });

            return Ok(result);
        }
    }
}