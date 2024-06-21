using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Options;
using StockMarket.Api.Contracts.Models.Requests;
using StockMarket.Api.Contracts.Models.Responses;
using StockMarket.Infrastructure.Exceptions;
using StockMarket.Infrastructure.Models;
using StockMarket.Infrastructure.Options;
using StockMarket.Infrastructure.Services.Abstractions;
using System.Text;
using System.Text.Json;
using System.Web;
using static StockMarket.Api.Contracts.Constants;

namespace StockMarket.Infrastructure.Services.Implementations
{
    public class StockMarketService : IStockMarket
    {
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AlphaVantageConfigurationsOptions _alphaVantageConfigurationsOptions;
        private readonly CexConfigurationsOptions _cexConfigurationsOptions;
         private readonly IValidator<TickerRequestModel> _validator;

        public StockMarketService(IHttpClientFactory httpClientFactory,
            IOptions<AlphaVantageConfigurationsOptions> alphaVantageConfigurationsOptions,
            IOptions<CexConfigurationsOptions> cexConfigurationsOptions,
            IValidator<TickerRequestModel> validator,
            IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _alphaVantageConfigurationsOptions = alphaVantageConfigurationsOptions.Value;
            _cexConfigurationsOptions = cexConfigurationsOptions.Value;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<dynamic> GetCurrenciesAsync(TickerRequestModel model ,CancellationToken cancellationToken = default)
        {
            //validation
            await _validator.ValidateAndThrowAsync(model, cancellationToken);

            var uriBuilder = new UriBuilder(_cexConfigurationsOptions.BaseAddress);
            uriBuilder.Path = Path.Combine(uriBuilder.Path, _cexConfigurationsOptions.FunctionName);

            var jsonString = new StringContent(
             JsonSerializer.Serialize(model),
             Encoding.UTF8,
             "application/json");

            using var httpClient = _httpClientFactory.CreateClient();

            using var httpResponseMessage =
                   await httpClient.PostAsync(uriBuilder.Uri, jsonString, cancellationToken);

            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                    
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var result = await JsonSerializer.DeserializeAsync<dynamic>(contentStream, cancellationToken: cancellationToken);

            return result;
        }

        public async Task<StockMarketResponseModel> GetLivePriceAsync(string fromCurrency, string toCurrency, CancellationToken cancellationToken)
        {
            var uriBuilder = new UriBuilder(_alphaVantageConfigurationsOptions.BaseAddress);
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters[FunctionName] = _alphaVantageConfigurationsOptions.FunctionName;
            parameters[FromCurrency] = fromCurrency;
            parameters[ToCurrency] = toCurrency;
            parameters[Apikey] = _alphaVantageConfigurationsOptions.ApiKey;
            uriBuilder.Query = parameters.ToString();

            using var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                uriBuilder.Uri);

            using var httpClient = _httpClientFactory.CreateClient();
            
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, cancellationToken);

            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            
            var result = await JsonSerializer.DeserializeAsync<CurrencyModel>(contentStream);
                  
            if (result.RealtimeCurrencyExchangeRate == null)
            {
                throw new TooManyRequestsException(result.Information);
            }
            else
            {
                return _mapper.Map<StockMarketResponseModel>(result);
            }
        }
    }
}
