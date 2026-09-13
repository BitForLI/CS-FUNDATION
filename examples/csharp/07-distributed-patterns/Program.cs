// Layer: distributed reliability. Run: dotnet run --project examples/csharp/07-distributed-patterns

var idempotency = new IdempotencyStore();
Console.WriteLine(await idempotency.ExecuteOnceAsync("request-42", () => Task.FromResult("order-created")));
Console.WriteLine(await idempotency.ExecuteOnceAsync("request-42", () => Task.FromResult("must-not-run")));

var attempt = 0;
var retryResult = await RetryAsync(async () =>
{
    await Task.Yield();
    attempt++;
    if (attempt < 3) throw new HttpRequestException("temporary failure");
    return "dependency recovered";
}, maxAttempts: 3, TimeSpan.FromMilliseconds(20));
Console.WriteLine($"Retry: {retryResult} after {attempt} attempts");

var cache = new CacheAside<string, string>(TimeSpan.FromMinutes(1));
var databaseReads = 0;
Task<string> LoadUser()
{
    databaseReads++;
    return Task.FromResult("Ada");
}
Console.WriteLine(await cache.GetAsync("user:1", LoadUser));
Console.WriteLine(await cache.GetAsync("user:1", LoadUser));
Console.WriteLine($"Database reads: {databaseReads} (expected 1)");

var breaker = new CircuitBreaker(failureThreshold: 2, openFor: TimeSpan.FromSeconds(1));
for (var i = 0; i < 3; i++)
{
    try { await breaker.ExecuteAsync<string>(() => throw new HttpRequestException("down")); }
    catch (Exception exception) { Console.WriteLine($"Circuit breaker: {exception.Message}"); }
}

static async Task<T> RetryAsync<T>(Func<Task<T>> operation, int maxAttempts, TimeSpan delay)
{
    for (var current = 1; ; current++)
    {
        try { return await operation(); }
        catch when (current < maxAttempts) { await Task.Delay(delay); }
    }
}

internal sealed class IdempotencyStore
{
    private readonly Dictionary<string, string> completed = [];

    public async Task<string> ExecuteOnceAsync(string key, Func<Task<string>> command)
    {
        if (completed.TryGetValue(key, out var previous)) return previous;
        var result = await command();
        completed[key] = result;
        return result;
    }
}

internal sealed class CacheAside<TKey, TValue>(TimeSpan ttl) where TKey : notnull
{
    private readonly Dictionary<TKey, (TValue Value, DateTimeOffset Expires)> entries = [];

    public async Task<TValue> GetAsync(TKey key, Func<Task<TValue>> loader)
    {
        if (entries.TryGetValue(key, out var hit) && hit.Expires > DateTimeOffset.UtcNow) return hit.Value;
        var value = await loader();
        entries[key] = (value, DateTimeOffset.UtcNow.Add(ttl));
        return value;
    }
}

internal sealed class CircuitBreaker(int failureThreshold, TimeSpan openFor)
{
    private int failures;
    private DateTimeOffset openUntil;

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation)
    {
        if (openUntil > DateTimeOffset.UtcNow) throw new InvalidOperationException("circuit is open");
        try
        {
            var result = await operation();
            failures = 0;
            return result;
        }
        catch
        {
            failures++;
            if (failures >= failureThreshold) openUntil = DateTimeOffset.UtcNow.Add(openFor);
            throw;
        }
    }
}

