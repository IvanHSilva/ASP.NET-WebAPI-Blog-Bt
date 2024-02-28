using Blog.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[ApiController]
[Route("v1")]
public class AccountController : ControllerBase {

    [HttpPost("login")]
    public IActionResult Login([FromServices]TokenService tokenService) { 
    
        string token = tokenService.GenerateToken(null!);
        
        return Ok(token); 
    }
}
