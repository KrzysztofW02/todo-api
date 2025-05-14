using Moq;
using todo_api.Models;
using todo_api.Repositories;
using todo_api.Services;
using todo_api.DTOs;
using todo_api.Mappings;
using Xunit;

namespace TodoApi.Tests
{
    public class TodoServiceTests
    {
        private readonly Mock<ITodoRepository> _repoMock;
        private readonly TodoService _service;

        public TodoServiceTests()
        {
            _repoMock = new(MockBehavior.Strict);
            _service = new TodoService(_repoMock.Object);
        }

        [Fact]
        public async Task GetAllItemsAsync_ReturnsMappedDtos()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var items = new List<TodoItem>
            {
                new() { Id = 1, Title = "TestTitle", Description = "Desc", DueDate = now, PercentComplete = 10, IsCompleted = false }
            };
            _repoMock.Setup(r => r.GetAllItemsAsync()).ReturnsAsync(items);

            // Act
            var result = await _service.GetAllItemsAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("TestTitle", result[0].Title);
            Assert.Equal("Desc", result[0].Description);
            Assert.Equal(now, result[0].DueDate);
            _repoMock.Verify(r => r.GetAllItemsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsDto_WhenFound()
        {
            // Arrange
            var item = new TodoItem { Id = 42, Title = "A", Description = "", DueDate = DateTime.UtcNow };
            _repoMock.Setup(r => r.GetByIdAsync(42)).ReturnsAsync(item);

            // Act
            var dto = await _service.GetByIdAsync(42);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(42, dto!.Id);
            _repoMock.Verify(r => r.GetByIdAsync(42), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((TodoItem?)null);

            // Act
            var dto = await _service.GetByIdAsync(99);

            // Assert
            Assert.Null(dto);
            _repoMock.Verify(r => r.GetByIdAsync(99), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_CreatesAndReturnsDto()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var createDto = new TodoCreateDto { Title = "C", Description = "D", DueDate = now };
            var entity = createDto.ToEntity();
            _repoMock.Setup(r => r.AddAsync(It.IsAny<TodoItem>())).Returns(Task.CompletedTask)
                     .Callback<TodoItem>(t => t.Id = 5);
            _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(createDto);

            // Assert
            Assert.Equal(5, result.Id);
            Assert.Equal("C", result.Title);
            _repoMock.Verify(r => r.AddAsync(It.IsAny<TodoItem>()), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((TodoItem?)null);

            // Act
            var ok = await _service.DeleteAsync(2);

            // Assert
            Assert.False(ok);
            _repoMock.Verify(r => r.GetByIdAsync(2), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenDeleted()
        {
            // Arrange
            var item = new TodoItem { Id = 3 };
            _repoMock.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(item);
            _repoMock.Setup(r => r.DeleteAsync(item)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var ok = await _service.DeleteAsync(3);

            // Assert
            Assert.True(ok);
            _repoMock.Verify(r => r.DeleteAsync(item), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        public async Task MarkAsCompleted_SetsCompleteAndReturnsTrue(int percentBefore)
        {
            // Arrange
            var item = new TodoItem { Id = 7, PercentComplete = percentBefore };
            _repoMock.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(item);
            _repoMock.Setup(r => r.UpdateAsync(item)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var ok = await _service.MarkAsCompleted(7);

            // Assert
            Assert.True(ok);
            Assert.True(item.IsCompleted);
            Assert.Equal(100, item.PercentComplete);
            _repoMock.Verify(r => r.UpdateAsync(item), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task SetPercentCompleteAsync_ReturnsFalse_WhenNotFound()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(8)).ReturnsAsync((TodoItem?)null);

            // Act
            var ok = await _service.SetPercentCompleteAsync(8, 30);

            // Assert
            Assert.False(ok);
            _repoMock.Verify(r => r.GetByIdAsync(8), Times.Once);
        }
    }
}
