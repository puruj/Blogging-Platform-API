namespace BloggingPlatform.API.Models.Entities
{
    public class BlogPost
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string Category { get; set; }
        public List<string>? Tags { get; set; }

    }
}
