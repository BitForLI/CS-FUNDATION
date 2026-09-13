namespace TaskBoard.Application;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}

