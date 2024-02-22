using BlogEFCore.Data;
using BlogEFCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
[Route("v1")]
public class CategoryController : ControllerBase {

    [HttpGet("categories")]
    public async Task<IActionResult> SelectAll ([FromServices] DataContext context) {
        List<Category> categories = await context.Categories.ToListAsync();
        return Ok(categories);
    }
}
