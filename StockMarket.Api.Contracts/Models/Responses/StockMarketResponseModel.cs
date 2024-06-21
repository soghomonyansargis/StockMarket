namespace StockMarket.Api.Contracts.Models.Responses
{
    public class StockMarketResponseModel
    {
        public string FromCurrencyCode { get; set; }
        public string FromCurrencyName { get; set; }
        public string ToCurrencyCode { get; set; }
        public string ToCurrencyName { get; set; }
        public string ExchangeRate { get; set; }
    }
}
