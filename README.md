# Blogging Platform API

ASP.NET Core Web API for a personal blogging platform with full CRUD and search over posts. Built with Entity Framework Core and SQL Server. Project implemented for https://roadmap.sh/projects/blogging-platform-api.

## Features
- Create, read (all/by id), update, delete blog posts
- Search posts by title/content/category/tags (case-insensitive)
- Swagger/OpenAPI UI for manual testing
- Async EF Core data access with validation on DTOs

## Tech Stack
- .NET 8, ASP.NET Core Web API
- Entity Framework Core
- SQL Server (development connection via `DefaultConnection`)
- xUnit + WebApplicationFactory + EFCore.InMemory for integration tests

## Prerequisites
- .NET 8 SDK
- SQL Server instance (localdb or full)
- Dev HTTPS certificate trusted: `dotnet dev-certs https --trust`

## Setup
1) Install dependencies: `dotnet restore`
2) Configure connection string in `BloggingPlatform.API/appsettings.json` under `DefaultConnection`.
3) Apply migrations (if using SQL Server):  
   `cd BloggingPlatform.API/BloggingPlatform.API`  
   `dotnet ef database update`

## Run
From `BloggingPlatform.API/BloggingPlatform.API`:
- Debug/dev with Swagger: `dotnet run --launch-profile "https"` (or `http`)
- Swagger UI: `https://localhost:7020/swagger` (matching your profile/port)

## Tests
- Run all tests from solution root:  
  `cd BloggingPlatform.API`  
  `dotnet test`

## API Endpoints (summary)
- `GET /api/BlogPosts` – list posts
- `GET /api/BlogPosts/{id}` – get by id
- `GET /api/BlogPosts/search?term=foo` – search
- `POST /api/BlogPosts` – create
- `PUT /api/BlogPosts/{id}` – update
- `DELETE /api/BlogPosts/{id}` – delete

## Notes
- Validation uses data annotations on DTOs; invalid payloads return 400.
- For large datasets, add paging/sorting to list/search endpoints.
