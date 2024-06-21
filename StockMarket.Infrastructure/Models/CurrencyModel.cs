using System.Text.Json.Serialization;

namespace StockMarket.Infrastructure.Models
{
    public class CurrencyModel
    {
        [JsonPropertyName("Realtime Currency Exchange Rate")]
        public RealtimeCurrencyExchangeRateModel RealtimeCurrencyExchangeRate { get; set; }

        public string Information { get; set; }
    }
}