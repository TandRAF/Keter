// Controllers/AuthController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.Dtos.UserDto;
using server.Interfaces;
using server.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace server.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
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

        [HttpPatch("update-avatar")]
        [Authorize] 
        public async Task<IActionResult> UpdateAvatar([FromBody] UpdateAvatarRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) 
                return Unauthorized(new { Message = "Acces refuzat. Token invalid." });
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) 
                return NotFound(new { Message = "Userul nu a fost găsit." });
            user.ProfilePictureUrl = request.AvatarUrl;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new { 
                    Message = "Poza de profil a fost actualizată cu succes!", 
                    Url = user.ProfilePictureUrl 
                });
            }

            return BadRequest(new { Message = "A apărut o eroare la salvarea pozei în baza de date." });
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string q){
            if (string.IsNullOrWhiteSpace(q) || q.Length < 2) 
                return Ok(new List<object>());
            var searchTerm = q.ToLower();
            var users = await _userManager.Users
            .Where(u => u.UserName.ToLower().Contains(searchTerm))
            .Take(5)
            .Select(u => new { u.Id, u.UserName })
            .ToListAsync();
            return Ok(users);
        }
    }
}