using StockMarket.Infrastructure.Options;
using StockMarket.Infrastructure.Services.Abstractions;
using StockMarket.Infrastructure.Services.Implementations;
using StockMarketWebSocket.Mapping;
using StockMarketWebSocket.Services.Abstractions;
using StockMarketWebSocket.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:6565");
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

builder.Services.AddScoped<IBroadcastService, BroadcastService>();
builder.Services.AddScoped<IStockMarket, StockMarketService>()
    .Configure<AlphaVantageConfigurationsOptions>(builder.Configuration.GetSection(nameof(AlphaVantageConfigurationsOptions)));

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.UseWebSockets();

await app.RunAsync();
