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
    public async Task<IActionResult> GetAll([FromServices] DataContext context) {
        //List<ListPostsViewModel>
        List<Post> posts = await context.Posts.AsNoTracking().
            Include(p => p.Category).Include(p => p.Author).
            //Select(p => new ListPostsViewModel {
            //    Id = p.Id,
            //    Title = p.Title,
            //    Slug = p.Slug,
            //    LastUpdateDate = p.LastUpdateDate,
            //    Category = p.Category.Name,
            //    Author = p.Author.Name
            //}).
            ToListAsync();
        return Ok(posts);
    }
}
