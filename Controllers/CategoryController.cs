﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;

namespace Blog.Controllers;

[ApiController]
[Route("v1")]
public class CategoryController : ControllerBase
{

    [HttpGet("categories")]
    public IActionResult GetAll(
        [FromServices] IMemoryCache cache, [FromServices] DataContext context)
    {
        try
        {
            List<Category>? categories = cache.GetOrCreate("CategoriesCache", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                return GetCategories(context);
            });

            return Ok(new ResultViewModel<List<Category>>(categories!));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("Falha interna do servidor"));
        }
    }

    private static List<Category> GetCategories(DataContext context)
    {
        return [.. context.Categories];
    }

    [HttpGet("categories/{id:int}")]
    public async Task<IActionResult> Get([FromRoute] int id, [FromServices] DataContext context)
    {
        Category? category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if (category == null) return NotFound(new ResultViewModel<Category>("Categoria não encontrada!"));

        return Ok(new ResultViewModel<Category>(category));
    }

    [HttpPost("categories")]
    public async Task<IActionResult> Post([FromBody] Category model, [FromServices] DataContext context)
    {

        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

        try
        {
            Category category = new() { Name = model.Name, Slug = model.Slug.ToLower() };
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return Created($"categories/{category.Id}", new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException dbex)
        {
            return StatusCode(500, new ResultViewModel<Category>($"Erro ao inserir categoria: {dbex}"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<Category>($"Falha interna do servidor: {ex}"));
        }
    }

    [HttpPut("categories/{id:int}")]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Category model,
        [FromServices] DataContext context)
    {

        Category category = await context.Categories.FirstAsync(c => c.Id == id);
        if (category == null) return NotFound(new ResultViewModel<Category>("Categoria não encontrada!"));

        category.Name = model.Name!;
        category.Slug = model.Slug!;

        context.Categories.Update(category);
        await context.SaveChangesAsync();

        return Ok(new ResultViewModel<Category>(category));
    }

    [HttpDelete("categories/{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id, [FromServices] DataContext context)
    {
        Category category = await context.Categories.FirstAsync(c => c.Id == id);
        if (category == null) return NotFound(new ResultViewModel<Category>("Categoria não encontrada!"));

        context.Categories.Remove(category);
        await context.SaveChangesAsync();

        return Ok(new ResultViewModel<Category>(category));
    }
}
