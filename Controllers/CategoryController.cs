using Blog.ViewModels;
using BlogEFCore.Data;
using BlogEFCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
[Route("v1")]
public class CategoryController : ControllerBase {

    [HttpGet("categories")]
    public async Task<IActionResult> GetAll ([FromServices] DataContext context) {
        List<Category> categories = await context.Categories.ToListAsync();
        
        return Ok(categories);
    }

    [HttpGet("categories/{id:int}")]
    public async Task<IActionResult> Get ([FromRoute] int id, [FromServices] DataContext context) {
        Category? category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if (category == null) return NotFound();
        
        return Ok(category);
    }

    [HttpPost("categories")]
    public async Task<IActionResult> Post([FromBody] CategoryViewModel model, [FromServices] DataContext context) {
        try {
            Category category = new() { Name = model.Name, Slug = model.Slug.ToLower()};
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return Created($"categories/{category.Id}", category);
        } catch (DbUpdateException dbex) {
            return StatusCode(500, $"Erro ao inserir categoria: {dbex}");
        } catch (Exception ex) {
            return StatusCode(500, $"Falha interna do servidor: {ex}");
        }
    }

    [HttpPut("categories/{id:int}")]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] CategoryViewModel model, 
        [FromServices] DataContext context) {
        Category category = await context.Categories.FirstAsync(c => c.Id == id);
        if (category == null) return NotFound();

        category.Name = model.Name!;
        category.Slug = model.Slug!;

        context.Categories.Update(category);
        await context.SaveChangesAsync();

        return Ok(category);
    }

    [HttpDelete("categories/{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id, [FromServices] DataContext context) {
        Category category = await context.Categories.FirstAsync(c => c.Id == id);
        if (category == null) return NotFound();

        context.Categories.Remove(category);
        await context.SaveChangesAsync();

        return Ok(category);
    }
}
