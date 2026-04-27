using FleetFactory.Application.Features.Auth.DTOs;
using FleetFactory.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetFactory.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService _authService) : ControllerBase
    {   
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var result  = await _authService.RegisterAsync(request);

            if (!result.Success) return BadRequest(result);

            return Created("", result); 
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);
            if (!result.Success) return Unauthorized(result);

            return Ok(result);
        }
    }
}