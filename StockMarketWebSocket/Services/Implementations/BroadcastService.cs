using Microsoft.Extensions.Caching.Memory;
using StockMarket.Api.Contracts.Models.Responses;
using StockMarket.Infrastructure.Services.Abstractions;
using StockMarketWebSocket.Services.Abstractions;
using System.Net.WebSockets;
using System.Text;

namespace StockMarketWebSocket.Services.Implementations
{
    public class BroadcastService : IBroadcastService
    {
        private readonly IStockMarket _stockMarket;
        private readonly IMemoryCache _cache;

        public BroadcastService(IStockMarket stockMarket,
            IMemoryCache cache)
        {
            _stockMarket = stockMarket;
            _cache = cache;
        }

        public async Task ExecuteAsync(WebSocket webSocket, string fromCurrency, string toCurrency, CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Fetch live prices from the data provider
                var prices = await GetCachedPricesAsync(fromCurrency, toCurrency, stoppingToken);

               await Echo(webSocket, prices, stoppingToken);

            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); // Adjust the delay as needed 
        }

        private async Task<StockMarketResponseModel> GetCachedPricesAsync(string fromCurrency, string toCurrency, CancellationToken stoppingToken)
        {
            // Cache the data for 10 seconds to reduce load on the data provider
            return await _cache.GetOrCreateAsync("LivePrices", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                return await _stockMarket.GetLivePriceAsync(fromCurrency, toCurrency, stoppingToken);
            });
        }

        private async Task Echo(WebSocket webSocket, StockMarketResponseModel model, CancellationToken cancellationToken)
        {
            try
            {
                var message = $"FromCurrencyName: {model.FromCurrencyCode}, ToCurrencyCode: {model.ToCurrencyCode}, ExchangeRate: {model.ExchangeRate}";
                var buffer = Encoding.UTF8.GetBytes(message);
                var arraySegment = new ArraySegment<byte>(buffer, 0, buffer.Length);
                while (true)
                {
                    if (webSocket.State == WebSocketState.Open)
                    {
                        await webSocket.SendAsync(
                       arraySegment,
                         WebSocketMessageType.Text,
                                   true,
                        cancellationToken);
                    }
                    else if (webSocket.State == WebSocketState.Closed || webSocket.State == WebSocketState.Aborted)
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                }

            }
            catch (Exception ex)
            {
                await webSocket.CloseAsync(
                               WebSocketCloseStatus.InternalServerError,
                               ex.Message,
                               cancellationToken);

                throw;
            }
        }
    }
}
