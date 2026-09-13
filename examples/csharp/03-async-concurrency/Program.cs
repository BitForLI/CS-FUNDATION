using System.Threading.Channels;

// Layer: runtime/concurrency. Run: dotnet run --project examples/csharp/03-async-concurrency

using var timeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(120));
try
{
    await SlowOperationAsync(timeout.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Cancellation bounded a slow operation.");
}

var counter = new SafeCounter();
await Task.WhenAll(Enumerable.Range(0, 8).Select(_ => Task.Run(() =>
{
    for (var i = 0; i < 10_000; i++) counter.Increment();
})));
Console.WriteLine($"Locked shared counter: {counter.Value} (expected 80000)");

var channel = Channel.CreateBounded<string>(2);
var consumer = ConsumeAsync(channel.Reader);
foreach (var job in new[] { "validate", "charge", "notify" }) await channel.Writer.WriteAsync(job);
channel.Writer.Complete();
await consumer;

static async Task SlowOperationAsync(CancellationToken cancellationToken)
{
    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
}

static async Task ConsumeAsync(ChannelReader<string> reader)
{
    await foreach (var job in reader.ReadAllAsync())
    {
        Console.WriteLine($"Consumed: {job}");
    }
}

internal sealed class SafeCounter
{
    private readonly object gate = new();
    private int value;

    public int Value
    {
        get { lock (gate) return value; }
    }

    public void Increment()
    {
        lock (gate) value++;
    }
}

