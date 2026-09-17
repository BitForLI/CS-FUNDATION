// Run: dotnet run --project examples/csharp/09-runtime-semantics
// Values are copied; variables holding references can point to the same mutable object.
var number = 10;
var copiedNumber = number;
copiedNumber++;
Check(number == 10 && copiedNumber == 11, "value copy / 值复制");

var original = new Box { Value = 10 };
var alias = original;
alias.Value++;
Check(original.Value == 11, "shared mutable object / 共享可变对象");

// A class can still offer value-based equality; record is one convenient option.
Check(new Coordinate(1, 2) == new Coordinate(1, 2), "record value equality / 记录值相等");

// try/finally (and using) release resources deterministically; GC timing is not a file-lock policy.
var lease = new TrackedResource();
using (lease) Console.WriteLine("Resource is in use.");
Check(lease.IsDisposed, "deterministic disposal / 确定性释放");

// Task.WhenAll waits for asynchronous work; it does not guarantee CPU parallelism.
var results = await Task.WhenAll(Enumerable.Range(1, 3).Select(async i =>
{
    await Task.Delay(10);
    return i * i;
}));
Check(results.SequenceEqual([1, 4, 9]), "async coordination / 异步协调");

static void Check(bool condition, string label)
{
    if (!condition) throw new Exception($"Failed: {label}");
    Console.WriteLine($"PASS {label}");
}

internal sealed class Box { public int Value { get; set; } }
internal sealed record Coordinate(int X, int Y);
internal sealed class TrackedResource : IDisposable
{
    public bool IsDisposed { get; private set; }
    public void Dispose() => IsDisposed = true;
}
