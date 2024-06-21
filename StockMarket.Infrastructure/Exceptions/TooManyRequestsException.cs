namespace StockMarket.Infrastructure.Exceptions
{
    public class TooManyRequestsException(string message) : Exception(message)
    {
    }
}
