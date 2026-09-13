using TaskBoard.Domain;

namespace TaskBoard.Application;

public interface ITodoRepository
{
    Task<IReadOnlyList<TodoItem>> ListAsync(CancellationToken cancellationToken);
    Task<TodoItem?> FindAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(TodoItem item, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}

