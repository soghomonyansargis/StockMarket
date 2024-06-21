using System.Net.WebSockets;
using System.Text;

internal class Program
{
    private static async Task Main(string[] args)
    {
        using var ws = new ClientWebSocket();

        try
        {
            await RunWebSocket(ws);
            await ReceiveResponse(ws);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static async Task RunWebSocket(ClientWebSocket ws)
    {
        await ws.ConnectAsync(new Uri("ws://localhost:6565/ws"), CancellationToken.None);
        await Console.Out.WriteLineAsync("Connected!");
    }

    private static async Task ReceiveResponse(ClientWebSocket ws)
    {
        var buffer = new byte[1024];
        while (true)
        {
            var resultFromSocket = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (resultFromSocket.MessageType == WebSocketMessageType.Close)
            {
                break;
            }

            var message = Encoding.UTF8.GetString(buffer, 0, resultFromSocket.Count);
            Console.WriteLine(message);
        }
    }
}