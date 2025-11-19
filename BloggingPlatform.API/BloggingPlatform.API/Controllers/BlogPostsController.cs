using BloggingPlatform.API.Data;
using BloggingPlatform.API.Models;
using BloggingPlatform.API.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloggingPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public BlogPostsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetBlogPosts()
        {
            var blogPosts = dbContext.BlogPosts.ToList();
            return Ok(blogPosts);
        }

        [HttpPost]
        public IActionResult CreateBlogPost(CreateBlogPostDto createBlogPostDto)
        {
            var blogPostentity = new BlogPost
            {
                Title = createBlogPostDto.Title,
                Content = createBlogPostDto.Content,
                Category = createBlogPostDto.Category,
                Tags = createBlogPostDto.Tags
            };

            dbContext.Add(blogPostentity);
            dbContext.SaveChanges();

            return Ok(blogPostentity);
        }
    }
}
