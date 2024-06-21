using StockMarket.Api.Contracts.Models.Requests;
using StockMarket.Api.Contracts.Models.Responses;

namespace StockMarket.Infrastructure.Services.Abstractions
{
    public interface IStockMarket
    {
        Task<dynamic> GetCurrenciesAsync(TickerRequestModel model, CancellationToken  cancellationToken = default);

        Task<StockMarketResponseModel> GetLivePriceAsync(string fromCurrency, string toCurrency, CancellationToken  cancellationToken = default);
    }
}