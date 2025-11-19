using System.ComponentModel.DataAnnotations;

namespace BloggingPlatform.API.Models
{
    public class CreateBlogPostDto
    {
        [Required, StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required, MinLength(10)]
        public string Content { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [MinLength(1)]
        public List<string>? Tags { get; set; }
    }
}
