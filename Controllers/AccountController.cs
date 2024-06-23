using Blog.Extensions;
using Blog.Services;
using Blog.ViewModels;
using Blog.ViewModels.Accounts;
using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using System.Text.RegularExpressions;

namespace Blog.Controllers;

[ApiController]
[Route("v1")]
public class AccountController : ControllerBase
{

    [HttpPost("accounts")]
    public async Task<IActionResult> Post([FromBody] RegisterViewModel model,
        [FromServices] EmailService emailService, [FromServices] DataContext context)
    {

        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        User user = new()
        {
            Name = model.Name,
            Email = model.Email,
            Slug = model.Email.Replace("@", "-").Replace(".", "-").ToLower(),
        };

        string password = PasswordGenerator.Generate(25);
        user.PasswordHash = PasswordHasher.Hash(password);

        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            emailService.Send(user.Name, user.Email, "Bem-vindo ao nosso blog.",
                $"Sua senha é {password}");

            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email,
                password
            }));

        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("E-Mail ja cadastrado"));
        }
    }

    //[AllowAnonymous]
    [HttpPost("accounts/login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model,
        [FromServices] DataContext context,
        [FromServices] TokenService tokenService)
    {

        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        User user = await context.Users.AsNoTracking(). //Include(u => u.Roles).
            FirstAsync(u => u.Email == model.Email);

        if (user == null)
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos!"));

        if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos!"));

        try
        {
            string token = tokenService.GenerateToken(user);
            return Ok(new ResultViewModel<string>(token, null!));
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna do servidor"));
        }
    }

    [Authorize]
    [HttpPost("accounts/upload-image")]
    public async Task<IActionResult> UploadImage(
        [FromBody] UploadImageViewModel model,
        [FromServices] DataContext context)
    {

        string fileName = $"{Guid.NewGuid().ToString()}.jpg";
        string data = new Regex(@"^data image[a-z]+;base64,").
            Replace(model.Base64Image, "");
        byte[] bytes = Convert.FromBase64String(data);

        try
        {
            await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>
                ("Falha interna do servidor"));
        }

        User? user = await context.Users.FirstOrDefaultAsync(u => u.Email ==
        User.Identity!.Name);

        if (user == null) return NotFound(new ResultViewModel<Category>
        ("Usuário não encontrado"));

        user.Image = $"https://localhost:0000/images/{fileName}";

        try
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>
                ("Falha interna do servidor"));
        }

        return Ok(new ResultViewModel<string>("Imagem alterada com sucesso!"));

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
}
