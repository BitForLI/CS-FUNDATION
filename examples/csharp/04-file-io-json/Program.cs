using System.Text.Json;

// Layer: OS/file boundary. Run: dotnet run --project examples/csharp/04-file-io-json

var original = new Settings("https://localhost:5080", RetryCount: 3);
var json = JsonSerializer.Serialize(original, new JsonSerializerOptions { WriteIndented = true });
var path = Path.Combine(Path.GetTempPath(), "cs-fundation-settings.json");

await File.WriteAllTextAsync(path, json);
Settings? restored;
await using (var stream = File.OpenRead(path))
{
    restored = await JsonSerializer.DeserializeAsync<Settings>(stream);
}

Console.WriteLine($"Path: {path}");
Console.WriteLine($"Round trip equal: {original == restored}");
File.Delete(path);

internal sealed record Settings(string ApiBase, int RetryCount);
