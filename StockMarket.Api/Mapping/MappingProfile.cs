using AutoMapper;
using StockMarket.Api.Contracts.Models.Responses;
using StockMarket.Infrastructure.Models;

namespace StockMarket.Api.Mapping
{
    /// <summary>
    /// Mapping profile.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<CurrencyModel, StockMarketResponseModel>()
                 .ForMember(dest => dest.FromCurrencyCode, opt => opt.MapFrom(src => src.RealtimeCurrencyExchangeRate.FromCurrencyCode))
                 .ForMember(dest => dest.ToCurrencyCode, opt => opt.MapFrom(src => src.RealtimeCurrencyExchangeRate.ToCurrencyCode))
                 .ForMember(dest => dest.ExchangeRate, opt => opt.MapFrom(src => src.RealtimeCurrencyExchangeRate.ExchangeRate))
                 .ForMember(dest => dest.FromCurrencyName, opt => opt.MapFrom(src => src.RealtimeCurrencyExchangeRate.FromCurrencyName))
                 .ForMember(dest => dest.ToCurrencyName, opt => opt.MapFrom(src => src.RealtimeCurrencyExchangeRate.ToCurrencyName));
        }
    }
}
