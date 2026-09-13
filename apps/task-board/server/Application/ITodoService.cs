using TaskBoard.Domain;

namespace TaskBoard.Application;

public interface ITodoService
{
    Task<IReadOnlyList<TodoItem>> ListAsync(CancellationToken cancellationToken);
    Task<TodoItem> CreateAsync(string title, CancellationToken cancellationToken);
    Task<TodoItem?> RenameAsync(Guid id, string title, CancellationToken cancellationToken);
    Task<TodoItem?> ToggleAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}

