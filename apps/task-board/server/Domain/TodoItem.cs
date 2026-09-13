namespace TaskBoard.Domain;

public sealed class TodoItem
{
    private TodoItem(Guid id, string title, DateTimeOffset createdAtUtc)
    {
        Id = id;
        Title = title;
        CreatedAtUtc = createdAtUtc;
    }

    public Guid Id { get; }
    public string Title { get; private set; }
    public bool IsDone { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; }

    public static TodoItem Create(string title, DateTimeOffset createdAtUtc) =>
        new(Guid.NewGuid(), NormalizeTitle(title), createdAtUtc);

    public void Rename(string title) => Title = NormalizeTitle(title);

    public void Toggle() => IsDone = !IsDone;

    private static string NormalizeTitle(string? title)
    {
        var normalized = title?.Trim() ?? string.Empty;
        if (normalized.Length is < 1 or > 200)
            throw new ArgumentException("Title must contain 1-200 characters.", nameof(title));
        return normalized;
    }
}

