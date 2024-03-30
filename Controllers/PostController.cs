using Blog.ViewModels;
using Blog.ViewModels.Posts;
using BlogEFCore.Data;
using BlogEFCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
[Route("v1")]
public class PostController : ControllerBase {

    [HttpGet("posts")]
    public async Task<IActionResult> GetAll([FromServices] DataContext context,
        [FromQuery] int page = 0, [FromQuery] int pageSize = 25) {
        //List<ListPostsViewModel>
        try {
            int count = await context.Posts.AsNoTracking().CountAsync();
            List<ListPostsViewModel> posts = await context.Posts.AsNoTracking().
                Include(p => p.Category).Include(p => p.Author).
                Select(p => new ListPostsViewModel {
                    Id = p.Id,
                    Title = p.Title,
                    Slug = p.Slug,
                    LastUpdateDate = p.LastUpdateDate,
                    Category = p.Category.Name,
                    Author = p.Author.Name
                }).
                Skip(page * pageSize).Take(pageSize).
                OrderByDescending(p => p.LastUpdateDate).
                ToListAsync();

            return Ok(new ResultViewModel<dynamic>(new {
                total = count, page, pageSize, posts
            }));
        } catch {
            return StatusCode(500, new ResultViewModel<List<Category>>("Falha interna do servidor"));
        }
    }

    [HttpGet("posts/{id:int}")]
    public async Task<IActionResult> Detail([FromServices] DataContext context,
    [FromRoute] int id) {
        try {
            Post? post = await context.Posts.AsNoTracking().
                Include(p => p.Author).ThenInclude(p => p.Roles).
                Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
                return NotFound(new ResultViewModel<Post>("Conteúdo não encontrado"));

            return Ok(new ResultViewModel<Post>(post));

        } catch (Exception ex) {
            return StatusCode(500, new ResultViewModel<Post>("Falha interna do servidor"));
        }
    }

    [HttpGet("posts/category/{category}")]
    public async Task<IActionResult> GetByCategory(
        [FromRoute] string category, [FromServices] DataContext context,
        [FromQuery] int page = 0, [FromQuery] int pageSize = 25) {

        try {
            int count = await context.Posts.AsNoTracking().CountAsync();
            List<ListPostsViewModel> posts = await context.Posts.AsNoTracking().
                Include(p => p.Category).Include(p => p.Author).
                Where(p => p.Category.Slug == category).
                Select(p => new ListPostsViewModel {
                    Id = p.Id,
                    Title = p.Title,
                    Slug = p.Slug,
                    LastUpdateDate = p.LastUpdateDate,
                    Category = p.Category.Name,
                    Author = p.Author.Name
                }).
                Skip(page * pageSize).Take(pageSize).
                OrderByDescending(p => p.LastUpdateDate).
                ToListAsync();

            return Ok(new ResultViewModel<dynamic>(new {
                total = count, page, pageSize, posts
            }));
        } catch {
            return StatusCode(500, new ResultViewModel<List<Category>>("Falha interna do servidor"));
        }
    }
}
