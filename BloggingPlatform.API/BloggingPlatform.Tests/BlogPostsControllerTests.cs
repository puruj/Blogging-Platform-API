using System.Net;
using System.Net.Http.Json;
using BloggingPlatform.API.Models;
using BloggingPlatform.API.Models.Entities;

namespace BloggingPlatform.Tests;

public class BlogPostsControllerTests
{
    [Fact]
    public async Task Create_ReturnsCreated_And_Persists()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();

        var dto = new CreateBlogPostDto
        {
            Title = "Test Post",
            Content = "This is a test blog post content.",
            Category = "Testing",
            Tags = new() { "tag1", "tag2" }
        };

        var response = await client.PostAsJsonAsync("/api/BlogPosts", dto);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<BlogPost>();
        Assert.NotNull(created);
        Assert.Equal(dto.Title, created!.Title);
        Assert.NotEqual(Guid.Empty, created.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_ForMissing()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync($"/api/BlogPosts/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_ChangesFields()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();

        var createDto = new CreateBlogPostDto
        {
            Title = "Original",
            Content = "Original content body.",
            Category = "Cat",
            Tags = new() { "one" }
        };

        var createdResponse = await client.PostAsJsonAsync("/api/BlogPosts", createDto);
        createdResponse.EnsureSuccessStatusCode();
        var created = await createdResponse.Content.ReadFromJsonAsync<BlogPost>();
        Assert.NotNull(created);
        Assert.NotEqual(Guid.Empty, created!.Id);

        var updateDto = new CreateBlogPostDto
        {
            Title = "Updated",
            Content = "Updated content body.",
            Category = "NewCat",
            Tags = new() { "two" }
        };

        var updateResponse = await client.PutAsJsonAsync($"/api/BlogPosts/{created!.Id}", updateDto);

        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
        var updated = await updateResponse.Content.ReadFromJsonAsync<BlogPost>();
        Assert.Equal(updateDto.Title, updated!.Title);
        Assert.Equal(updateDto.Category, updated.Category);
        Assert.Equal(updateDto.Tags, updated.Tags);
    }

    [Fact]
    public async Task Delete_RemovesPost()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();

        var dto = new CreateBlogPostDto
        {
            Title = "To Delete",
            Content = "Content to delete.",
            Category = "Cat",
            Tags = new() { "one" }
        };

        var createResponse = await client.PostAsJsonAsync("/api/BlogPosts", dto);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<BlogPost>();
        Assert.NotNull(created);
        Assert.NotEqual(Guid.Empty, created!.Id);

        var deleteResponse = await client.DeleteAsync($"/api/BlogPosts/{created!.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await client.GetAsync($"/api/BlogPosts/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task Search_FindsMatchingPosts()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();

        var posts = new[]
        {
            new CreateBlogPostDto { Title = "Hello World", Content = "Intro content long", Category = "General", Tags = new() { "hello" } },
            new CreateBlogPostDto { Title = "Advanced C#", Content = "Deep dive into C#", Category = "DotNet", Tags = new() { "csharp" } }
        };

        foreach (var dto in posts)
        {
            var resp = await client.PostAsJsonAsync("/api/BlogPosts", dto);
            resp.EnsureSuccessStatusCode();
        }

        var response = await client.GetAsync("/api/BlogPosts/search?term=hello");
        var body = await response.Content.ReadAsStringAsync();
        Assert.True(response.IsSuccessStatusCode, body);

        var results = await response.Content.ReadFromJsonAsync<List<BlogPost>>();
        Assert.NotNull(results);
        Assert.Single(results!);
        Assert.Contains(results!, r => r.Title.Contains("Hello"));
    }

    [Fact]
    public async Task Create_InvalidPayload_ReturnsBadRequest()
    {
        using var factory = new TestWebApplicationFactory();
        using var client = factory.CreateClient();

        var invalid = new CreateBlogPostDto
        {
            Title = "", // required
            Content = "short", // too short per validation
            Category = ""
        };

        var response = await client.PostAsJsonAsync("/api/BlogPosts", invalid);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
