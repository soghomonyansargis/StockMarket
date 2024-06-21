using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StockMarket.Api.Contracts.Errors;
using StockMarket.Api.Contracts.Models.Requests;
using StockMarket.Api.Contracts.Models.Responses;
using StockMarket.Api.Controllers.Base;
using StockMarket.Infrastructure.Services.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace StockMarket.Api.Controllers
{
    [SwaggerTag("Operations for managing products")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class CurrenciesController : BaseApiController
    {
        private readonly IStockMarket _stockMarketService;

        public CurrenciesController(IMapper mapper, IStockMarket stockMarketService) : base(mapper)
        {
            _stockMarketService = stockMarketService;
        }

        [HttpGet("price")]
        [SwaggerOperation("Get the current price of a specific financial instrument.")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(StockMarketResponseModel), Description = "Status Success")]
        [SwaggerResponse(StatusCodes.Status429TooManyRequests, Type = typeof(ErrorDetails), Description = "Too Many Requests")]
        public async Task<IActionResult> GetCurrentPriceAsync([FromQuery] string fromCurrency, [FromQuery] string toCurrency,CancellationToken cancellationToken)
        {
            var response = await _stockMarketService.GetLivePriceAsync(fromCurrency, toCurrency, cancellationToken);
            return Ok(response);
        }

        [HttpPost]
        [SwaggerOperation("Get the financial instruments.")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(StockMarketResponseModel), Description = "Status Success")]
        public async Task<IActionResult> GetCurrenciesAsync([FromBody]TickerRequestModel model, CancellationToken cancellationToken)
        {
            var response = await _stockMarketService.GetCurrenciesAsync(model, cancellationToken);
            return Ok(response);
        }
    }
}