using StockMarket.Api.Mapping;
using StockMarket.Api.Middleware;
using StockMarket.Infrastructure.Options;
using StockMarket.Infrastructure.Services.Abstractions;
using StockMarket.Infrastructure.Services.Implementations;
using FluentValidation;
using StockMarket.Api.Contracts.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<RouteOptions>(opt =>
{
    opt.LowercaseUrls = true;
    opt.LowercaseQueryStrings = true;
});

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
});

builder.Services.AddHttpClient();
builder.Services.AddScoped<IStockMarket, StockMarketService>()
    .Configure<AlphaVantageConfigurationsOptions>(builder.Configuration.GetSection(nameof(AlphaVantageConfigurationsOptions)))
    .Configure<CexConfigurationsOptions>(builder.Configuration.GetSection(nameof(CexConfigurationsOptions)));

builder.Services.AddValidatorsFromAssemblies([typeof(TickerRequestModelValidator).Assembly, typeof(Program).Assembly]);

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
