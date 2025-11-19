using BloggingPlatform.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BloggingPlatform.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<BlogPost> BlogPosts { get; set; }
    }
}
