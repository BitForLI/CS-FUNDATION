// Layer: algorithms/data structures. Run: dotnet run --project examples/csharp/02-data-structures

var stack = new Stack<string>();
stack.Push("page-1");
stack.Push("page-2");
Console.WriteLine($"Stack/LIFO: {stack.Pop()}");

var queue = new Queue<string>();
queue.Enqueue("order-1");
queue.Enqueue("order-2");
Console.WriteLine($"Queue/FIFO: {queue.Dequeue()}");

var usersById = new Dictionary<int, string> { [10] = "Ada", [20] = "Linus" };
Console.WriteLine($"Dictionary/key lookup: {usersById[20]}");

var sorted = new[] { 2, 5, 9, 12, 18, 21, 30 };
Console.WriteLine($"Binary search index of 18: {BinarySearch(sorted, 18)}");

var graph = new Dictionary<string, string[]>
{
    ["A"] = ["B", "C"],
    ["B"] = ["D"],
    ["C"] = ["D"],
    ["D"] = [],
};
Console.WriteLine($"BFS shortest route: {string.Join(" -> ", BreadthFirstPath(graph, "A", "D"))}");

static int BinarySearch(IReadOnlyList<int> sorted, int target)
{
    var low = 0;
    var high = sorted.Count - 1;
    while (low <= high)
    {
        var middle = low + (high - low) / 2;
        if (sorted[middle] == target) return middle;
        if (sorted[middle] < target) low = middle + 1;
        else high = middle - 1;
    }
    return -1;
}

static IReadOnlyList<string> BreadthFirstPath(
    IReadOnlyDictionary<string, string[]> graph,
    string start,
    string target)
{
    var queue = new Queue<string>();
    var parent = new Dictionary<string, string?> { [start] = null };
    queue.Enqueue(start);

    while (queue.Count > 0)
    {
        var current = queue.Dequeue();
        if (current == target) break;
        foreach (var next in graph[current])
        {
            if (!parent.TryAdd(next, current)) continue;
            queue.Enqueue(next);
        }
    }

    if (!parent.ContainsKey(target)) return [];
    var path = new List<string>();
    for (string? node = target; node is not null; node = parent[node]) path.Add(node);
    path.Reverse();
    return path;
}

