using FluentValidation;
using StockMarket.Api.Contracts.Models.Requests;

namespace StockMarket.Api.Contracts.Validation
{
    public class TickerRequestModelValidator: AbstractValidator<TickerRequestModel>
    {
        public TickerRequestModelValidator()
        {
            RuleFor(x => x.Pairs).NotEmpty();
            RuleFor(x => x).NotEmpty();
        }
    }
}
