using CryptoQuestService.Models.Settings;
using CryptoQuestService.Services;
using CryptoQuestService.Services.Caches;
using CryptoQuestService.Services.HostedServices;
using CryptoQuestService.Services.HttpClients;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ApiSettings>(builder.Configuration.GetRequiredSection(nameof(ApiSettings)));

// Custom services
builder.Services.AddTransient<ContractDeployer>();
builder.Services.AddTransient<TablelandEntitiesCacheService>();

// Http Services
builder.Services.AddHttpClient<TablelandHttpService>();

// Hosted services
builder.Services.AddHostedService<CryptoQuestReduxInteractionService>();

// Simple memory cache hookup
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Per start we'll init the cache w/ whatever tableland tables we have for our current controller aka owner
await CacheInit(app);

app.Run();

async Task CacheInit(WebApplication app)
{
    var cacheService = app.Services.GetRequiredService<TablelandEntitiesCacheService>();
    await cacheService.InitializeTables();
}