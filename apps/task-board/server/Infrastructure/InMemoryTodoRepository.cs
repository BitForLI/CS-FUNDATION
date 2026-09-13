using System.Collections.Concurrent;
using TaskBoard.Application;
using TaskBoard.Domain;

namespace TaskBoard.Infrastructure;

public sealed class InMemoryTodoRepository : ITodoRepository
{
    private readonly ConcurrentDictionary<Guid, TodoItem> items = new();

    public Task<IReadOnlyList<TodoItem>> ListAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IReadOnlyList<TodoItem> result = items.Values.OrderBy(item => item.CreatedAtUtc).ToList();
        return Task.FromResult(result);
    }

    public Task<TodoItem?> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        items.TryGetValue(id, out var item);
        return Task.FromResult(item);
    }

    public Task AddAsync(TodoItem item, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!items.TryAdd(item.Id, item)) throw new InvalidOperationException("Duplicate task ID.");
        return Task.CompletedTask;
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(items.TryRemove(id, out _));
    }
}

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}

