using Microsoft.AspNetCore.Mvc;
using MiniBanking.API.DTOs;
using MiniBanking.API.Services.Interfaces;

namespace MiniBanking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto registerDto)
        {
            try
            {
                var result = await _authService.Register(registerDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            var result = await _authService.Login(loginDto);

            if (result == null)
                return Unauthorized("Kullanıcı adı veya şifre yanlış.");

            return Ok(result);
        }
    }
}