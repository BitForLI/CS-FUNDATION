using System.Net;
using System.Net.Sockets;
using System.Text;

// Layer: networking. Run: dotnet run --project examples/csharp/05-networking

var listener = new TcpListener(IPAddress.Loopback, 0);
listener.Start();
var port = ((IPEndPoint)listener.LocalEndpoint).Port;
var server = EchoOnceAsync(listener);

using var client = new TcpClient();
await client.ConnectAsync(IPAddress.Loopback, port);
await using var stream = client.GetStream();
await WriteFrameAsync(stream, "hello over TCP");
Console.WriteLine($"Client received: {await ReadFrameAsync(stream)}");

await server;
listener.Stop();

static async Task EchoOnceAsync(TcpListener listener)
{
    using var connection = await listener.AcceptTcpClientAsync();
    await using var stream = connection.GetStream();
    var message = await ReadFrameAsync(stream);
    await WriteFrameAsync(stream, message.ToUpperInvariant());
}

static async Task WriteFrameAsync(Stream stream, string text)
{
    var body = Encoding.UTF8.GetBytes(text);
    var length = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(body.Length));
    await stream.WriteAsync(length);
    await stream.WriteAsync(body);
}

static async Task<string> ReadFrameAsync(Stream stream)
{
    var lengthBytes = new byte[sizeof(int)];
    await stream.ReadExactlyAsync(lengthBytes);
    var length = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(lengthBytes));
    if (length is < 0 or > 1_000_000) throw new InvalidDataException("Invalid frame length.");
    var body = new byte[length];
    await stream.ReadExactlyAsync(body);
    return Encoding.UTF8.GetString(body);
}

