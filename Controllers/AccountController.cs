using Blog.Extensions;
using Blog.Services;
using Blog.ViewModels;
using BlogEFCore.Data;
using BlogEFCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blog.Controllers;

[ApiController]
[Route("v1")]
public class AccountController : ControllerBase {

    [HttpPost("accounts")]
    public async Task<IActionResult> Post([FromBody]RegisterViewModel model,
        [FromServices]DataContext context) {

        if (!ModelState.IsValid) return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        User user = new() {
            Name = model.Name,
            Email = model.Email,
            Slug = model.Email.Replace("@","-").Replace(".","-").ToLower(),
        };

        string password = PasswordGenerator.Generate(25);
        user.PasswordHash = PasswordHasher.Hash(password);

        try {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<dynamic>(new { user = user.Email, password }));
        } catch (DbUpdateException) {
            return StatusCode(400, new ResultViewModel<string>("E-Mail ja cadastrado"));
        }
    }

    //[AllowAnonymous]
    [HttpPost("accounts/login")]
    public IActionResult Login([FromServices]TokenService tokenService) { 
    
        string token = tokenService.GenerateToken(null!);
        
        return Ok(token); 
    }

    //[Authorize(Roles = "user")]
    //[HttpGet("user")]
    //public IActionResult GetUser() => Ok(User.Identity!.Name);

    //[Authorize(Roles = "author")]
    //[HttpGet("author")]
    //public IActionResult GetAuthor() => Ok(User.Identity!.Name);

    //[Authorize(Roles = "admin")]
    //[HttpGet("admin")]
    //public IActionResult GetAdmin() => Ok(User.Identity!.Name);
}
