namespace StockMarket.Api.Contracts.Errors
{
    public class ModelNotValidErrorDetails : ErrorDetails
    {
        public IEnumerable<ValidationError> Errors { get; set; }
    }
}