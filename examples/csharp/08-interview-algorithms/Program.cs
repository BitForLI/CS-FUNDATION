// Run: dotnet run --project examples/csharp/08-interview-algorithms
// These four patterns recur in interviews; each includes a check and complexity note.

Check(TwoSum([2, 7, 11, 15], 9) is { } pair && pair == (0, 1), "hash lookup / 哈希查找");
Check(LongestUniqueSubstring("abcabcbb") == 3, "sliding window / 滑动窗口");
Check(ValidParentheses("{[()]}") && !ValidParentheses("([)]"), "stack / 栈");
Check(LowerBound([1, 2, 2, 2, 4], 2) == 1 && LowerBound([1, 2, 2], 3) == 3,
    "binary search boundary / 二分边界");
Console.WriteLine("All interview algorithm examples passed.");

// O(n) time and O(n) extra space; store the needed complement in a dictionary.
static (int First, int Second)? TwoSum(IReadOnlyList<int> values, int target)
{
    var positions = new Dictionary<int, int>();
    for (var index = 0; index < values.Count; index++)
    {
        if (positions.TryGetValue(target - values[index], out var first)) return (first, index);
        positions.TryAdd(values[index], index);
    }
    return null;
}

// O(n) time and O(min(n, alphabet size)) space; the left boundary never moves backward.
static int LongestUniqueSubstring(string text)
{
    var lastSeen = new Dictionary<char, int>();
    var left = 0;
    var best = 0;
    for (var right = 0; right < text.Length; right++)
    {
        if (lastSeen.TryGetValue(text[right], out var previous) && previous >= left) left = previous + 1;
        lastSeen[text[right]] = right;
        best = Math.Max(best, right - left + 1);
    }
    return best;
}

// O(n) time and O(n) space in the worst case. Push expected closing characters.
static bool ValidParentheses(string text)
{
    var expected = new Stack<char>();
    foreach (var character in text)
    {
        switch (character)
        {
            case '(': expected.Push(')'); break;
            case '[': expected.Push(']'); break;
            case '{': expected.Push('}'); break;
            case ')' or ']' or '}':
                if (expected.Count == 0 || expected.Pop() != character) return false;
                break;
            default: return false;
        }
    }
    return expected.Count == 0;
}

// O(log n) time and O(1) space; returns first position >= target, or Count.
static int LowerBound(IReadOnlyList<int> sorted, int target)
{
    var left = 0;
    var right = sorted.Count; // half-open interval [left, right)
    while (left < right)
    {
        var middle = left + (right - left) / 2;
        if (sorted[middle] < target) left = middle + 1;
        else right = middle;
    }
    return left;
}

static void Check(bool condition, string label)
{
    if (!condition) throw new Exception($"Failed: {label}");
    Console.WriteLine($"PASS {label}");
}
