using TaskBoard.Domain;

namespace TaskBoard.Application;

public sealed class TodoService(ITodoRepository repository, IClock clock) : ITodoService
{
    public Task<IReadOnlyList<TodoItem>> ListAsync(CancellationToken cancellationToken) =>
        repository.ListAsync(cancellationToken);

    public async Task<TodoItem> CreateAsync(string title, CancellationToken cancellationToken)
    {
        var item = TodoItem.Create(title, clock.UtcNow);
        await repository.AddAsync(item, cancellationToken);
        return item;
    }

    public async Task<TodoItem?> RenameAsync(Guid id, string title, CancellationToken cancellationToken)
    {
        var item = await repository.FindAsync(id, cancellationToken);
        if (item is null) return null;
        item.Rename(title);
        return item;
    }

    public async Task<TodoItem?> ToggleAsync(Guid id, CancellationToken cancellationToken)
    {
        var item = await repository.FindAsync(id, cancellationToken);
        if (item is null) return null;
        item.Toggle();
        return item;
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken) =>
        repository.DeleteAsync(id, cancellationToken);
}

