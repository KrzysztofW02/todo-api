using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using todo_api;
using todo_api.DTOs;
using todo_api.Models;
using Microsoft.AspNetCore.Hosting;
using Xunit;

namespace TodoApi.Tests
{
    public class TodoControllerIntegrationTests :
        IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public TodoControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<DbContextOptions<TodoDbContext>>();
                    services.RemoveAll<TodoDbContext>();

                    services.AddDbContext<TodoDbContext>(opt =>
                        opt.UseInMemoryDatabase("TestDb"));

                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                });
            });
        }

        [Fact]
        public async Task GetAll_ReturnsEmptyList_OnFreshDb()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/todo");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var list = await response.Content.ReadFromJsonAsync<List<TodoReadDto>>();
            Assert.NotNull(list);
            Assert.Empty(list);
        }

        [Fact]
        public async Task Post_ThenGetById_WorksCorrectly()
        {
            var client = _factory.CreateClient();

            var createDto = new TodoCreateDto
            {
                Title = "IntegrationTest",
                Description = "Description",
                DueDate = DateTime.UtcNow.AddDays(1)
            };
            var postResp = await client.PostAsJsonAsync("/api/todo", createDto);
            Assert.Equal(HttpStatusCode.Created, postResp.StatusCode);

            var created = await postResp.Content.ReadFromJsonAsync<TodoReadDto>();
            Assert.NotNull(created);
            Assert.Equal("IntegrationTest", created!.Title);

            var getResp = await client.GetAsync($"/api/todo/{created.Id}");
            Assert.Equal(HttpStatusCode.OK, getResp.StatusCode);

            var fetched = await getResp.Content.ReadFromJsonAsync<TodoReadDto>();
            Assert.Equal(created.Id, fetched!.Id);
            Assert.Equal("IntegrationTest", fetched.Title);
        }

        [Fact]
        public async Task Delete_RemovesItem_AndGetByIdReturnsNotFound()
        {
            var client = _factory.CreateClient();

            var createDto = new TodoCreateDto
            {
                Title = "ToBeDeleted",
                Description = "To delete",
                DueDate = DateTime.UtcNow.AddDays(2)
            };
            var post = await client.PostAsJsonAsync("/api/todo", createDto);
            Assert.Equal(HttpStatusCode.Created, post.StatusCode);

            var created = await post.Content.ReadFromJsonAsync<TodoReadDto>();
            Assert.NotNull(created);

            var del = await client.DeleteAsync($"/api/todo/{created.Id}");
            Assert.Equal(HttpStatusCode.NoContent, del.StatusCode);

            var get = await client.GetAsync($"/api/todo/{created.Id}");
            Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
        }
    }
}
