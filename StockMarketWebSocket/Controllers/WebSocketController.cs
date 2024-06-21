using Microsoft.AspNetCore.Mvc;
using StockMarketWebSocket.Services.Abstractions;

namespace StockMarketWebSocket.Controllers
{
    public class WebSocketController : ControllerBase
    {
        private readonly IBroadcastService _broadcastService;

        public WebSocketController(IBroadcastService broadcastService)
        {
            _broadcastService = broadcastService;
        }

        [Route("/ws")]
        public async Task Get(CancellationToken cancellationToken)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await _broadcastService.ExecuteAsync(webSocket, fromCurrency: "usd", toCurrency: "jpy", cancellationToken);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}