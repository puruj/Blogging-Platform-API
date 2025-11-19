using System.Threading.Tasks;
using BloggingPlatform.API.Data;
using BloggingPlatform.API.Models;
using BloggingPlatform.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // Returns all posts (add paging when the dataset grows).
        [HttpGet]
        public async Task<IActionResult> GetBlogPosts()
        {
            var blogPosts = await dbContext.BlogPosts.ToListAsync();
            return Ok(blogPosts);
        }

        // Retrieves a single post by id.
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBlogPost(Guid id)
        {
            var blogPost = await dbContext.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return Ok(blogPost);
        }

        // Free-text search across title/content/category/tags.
        [HttpGet("search")]
        public async Task<IActionResult> SearchBlogPosts([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest("Search term is required.");
            }

            var lowered = term.ToLowerInvariant();

            var results = await dbContext.BlogPosts
                .Where(p =>
                    p.Title.ToLower().Contains(lowered) ||
                    p.Content.ToLower().Contains(lowered) ||
                    p.Category.ToLower().Contains(lowered) ||
                    (p.Tags != null && p.Tags.Any(tag => tag.ToLower().Contains(lowered))))
                .ToListAsync();

            return Ok(results);
        }

        // Creates a new post.
        [HttpPost]
        public async Task<IActionResult> CreateBlogPost(CreateBlogPostDto createBlogPostDto)
        {
            var blogPostEntity = new BlogPost
            {
                Title = createBlogPostDto.Title,
                Content = createBlogPostDto.Content,
                Category = createBlogPostDto.Category,
                Tags = createBlogPostDto.Tags
            };

            await dbContext.BlogPosts.AddAsync(blogPostEntity);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBlogPost), new { id = blogPostEntity.Id }, blogPostEntity);
        }

        // Full update of an existing post.
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateBlogPost(Guid id, CreateBlogPostDto updateDto)
        {
            var blogPost = await dbContext.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            blogPost.Title = updateDto.Title;
            blogPost.Content = updateDto.Content;
            blogPost.Category = updateDto.Category;
            blogPost.Tags = updateDto.Tags;

            await dbContext.SaveChangesAsync();

            return Ok(blogPost);
        }

        // Deletes a post.
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteBlogPost(Guid id)
        {
            var blogPost = await dbContext.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            dbContext.BlogPosts.Remove(blogPost);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
