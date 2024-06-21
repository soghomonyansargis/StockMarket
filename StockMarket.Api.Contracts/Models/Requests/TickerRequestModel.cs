using System.Text.Json.Serialization;

namespace StockMarket.Api.Contracts.Models.Requests
{
    public class TickerRequestModel
    {
        [JsonPropertyName("pairs")]
        public IEnumerable<string> Pairs { get; set; }
    }
}
