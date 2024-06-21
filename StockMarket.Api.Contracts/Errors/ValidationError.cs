namespace StockMarket.Api.Contracts.Errors
{
    public class ValidationError
    {
        public string Field { get; set; }

        public string Value { get; set; }

        public string Issue { get; set; }
    }
}