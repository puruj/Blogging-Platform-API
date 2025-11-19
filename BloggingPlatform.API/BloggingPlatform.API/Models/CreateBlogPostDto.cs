namespace BloggingPlatform.API.Models
{
    public class CreateBlogPostDto
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string Category { get; set; }
        public List<string>? Tags { get; set; }
    }
}
