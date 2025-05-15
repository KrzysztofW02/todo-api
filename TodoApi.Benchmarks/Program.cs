using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Moq;
using todo_api.DTOs;
using todo_api.Models;
using todo_api.Repositories;
using todo_api.Services;

[SimpleJob(launchCount: 1, warmupCount: 1, iterationCount: 3)]
public class TodoServiceBenchmarks
{
    private TodoService _service = null!;
    private List<TodoItem> _manyItems = null!;

    [GlobalSetup]
    public void Setup()
    {
        _manyItems = Enumerable.Range(1, 100)
            .Select(i => new TodoItem { Id = i, Title = $"T{i}" })
            .ToList();

        var repo = new Mock<ITodoRepository>();
        repo.Setup(r => r.GetAllItemsAsync()).ReturnsAsync(_manyItems);

        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => new TodoItem { Id = id, Title = $"T{id}" });

        repo.Setup(r => r.AddAsync(It.IsAny<TodoItem>()))
            .Callback<TodoItem>(t => t.Id = 999)
            .Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        repo.Setup(r => r.UpdateAsync(It.IsAny<TodoItem>())).Returns(Task.CompletedTask);

        _service = new TodoService(repo.Object);
    }

    [Benchmark]
    public List<TodoReadDto> GetAllItems() =>
        _service.GetAllItemsAsync().GetAwaiter().GetResult();

    [Benchmark]
    public TodoReadDto? GetById() =>
        _service.GetByIdAsync(_manyItems.Count / 2).GetAwaiter().GetResult();

    [Benchmark]
    public void Create()
    {
        var dto = new TodoCreateDto
        {
            Title = "B",
            Description = "D",
            DueDate = DateTime.UtcNow
        };
        _service.CreateAsync(dto).GetAwaiter().GetResult();
    }

    [Benchmark]
    public void MarkAsCompleted()
    {
        var id = 123;
        _service.MarkAsCompleted(id).GetAwaiter().GetResult();
    }

    [Benchmark]
    public void SetPercentComplete()
    {
        var id = 123;
        _service.SetPercentCompleteAsync(id, 50).GetAwaiter().GetResult();
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<TodoServiceBenchmarks>();
    }
}
