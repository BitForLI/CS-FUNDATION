using TaskBoard.Application;
using TaskBoard.Infrastructure;

namespace TaskBoard.Tests;

public sealed class TodoServiceTests
{
    private static readonly DateTimeOffset FixedNow = new(2026, 1, 2, 3, 4, 5, TimeSpan.Zero);

    [Fact]
    public async Task Create_trims_title_and_uses_injected_clock()
    {
        var service = CreateService();

        var item = await service.CreateAsync("  learn layers  ", CancellationToken.None);

        Assert.Equal("learn layers", item.Title);
        Assert.Equal(FixedNow, item.CreatedAtUtc);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Create_rejects_empty_title(string title)
    {
        var service = CreateService();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateAsync(title, CancellationToken.None));
    }

    [Fact]
    public async Task Toggle_returns_null_for_unknown_id()
    {
        var service = CreateService();

        var result = await service.ToggleAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task Toggle_changes_domain_state()
    {
        var service = CreateService();
        var item = await service.CreateAsync("test", CancellationToken.None);

        var changed = await service.ToggleAsync(item.Id, CancellationToken.None);

        Assert.NotNull(changed);
        Assert.True(changed.IsDone);
    }

    private static TodoService CreateService() =>
        new(new InMemoryTodoRepository(), new FakeClock(FixedNow));

    private sealed class FakeClock(DateTimeOffset value) : IClock
    {
        public DateTimeOffset UtcNow => value;
    }
}

