using BlogEFCore.Data;
using BlogEFCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers; 

public class CategoryController : ControllerBase {

    [HttpGet("categories")]
    public IActionResult SelectAll([FromServices] DataContext context) {
        List<Category> categories = [.. context.Categories];
        return Ok(categories);
    }
}
