using System.Net.WebSockets;

namespace StockMarketWebSocket.Services.Abstractions
{
    public interface IBroadcastService
    {
        Task ExecuteAsync(WebSocket webSocket, string fromCurrency, string toCurrency, CancellationToken stoppingToken);
    }
}
